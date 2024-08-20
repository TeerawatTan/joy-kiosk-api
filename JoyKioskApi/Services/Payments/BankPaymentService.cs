using JoyKioskApi.Constants;
using JoyKioskApi.Dtos.Banks;
using JoyKioskApi.Helpers;
using JoyKioskApi.Models;
using JoyKioskApi.Repositories.Logings;
using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static JoyKioskApi.Dtos.Commons.ResponseDto;

namespace JoyKioskApi.Services.Payments
{
    public class BankPaymentService : IBankPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogTxnRepository _logTxnRepository;

        public BankPaymentService(IConfiguration configuration, ILogTxnRepository logTxnRepository)
        {
            _configuration = configuration;
            _logTxnRepository = logTxnRepository;
        }

        public async Task<ResultResponse> KBankOAuth()
        {
            try
            {
                string kbankUrl = Environment.GetEnvironmentVariable("KBanksUrl")!;
                string endpoint = "/v2/oauth/token";
                string consumerKey = Environment.GetEnvironmentVariable("KBanksKey")!;
                string consumerSecret = Environment.GetEnvironmentVariable("KBanksSecret")!;
                byte[] byteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", consumerKey, consumerSecret));
                string grantType = Environment.GetEnvironmentVariable("KBanksGrantType")!;

                HttpClient client = new();
                HttpRequestMessage request = new(HttpMethod.Post, kbankUrl + endpoint);

                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(byteArray));

                var collection = new List<KeyValuePair<string, string>>();
                collection.Add(new("grant_type", grantType));
                var content = new FormUrlEncodedContent(collection);

                request.Content = content;

                var responseMessage = await client.SendAsync(request);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Message = responseContent
                    };
                }

                var result = JsonSerializer.Deserialize<KbankResDto?>(responseContent);

                OAuthResDto response = new()
                {
                    TokenType = result?.TokenType,
                    AccessToken = result?.AccessToken,
                    ClientId = result?.ClientId,
                    ExpiresIn = result?.ExpiresIn,
                    Status = result?.Status
                };

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = response,
                    Message = AppConstant.STATUS_SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResultResponse> QRPaymentRequest(QRCodePaymentRequestReqDto req)
        {
            string methodName = GetCurrentAsyncMethod.GetName(MethodBase.GetCurrentMethod());

            try
            {
                string kbankUrl = Environment.GetEnvironmentVariable("KBanksUrl")!;
                string endpoint = "/v1/qrpayment/request";

                HttpClient client = new();
                client.BaseAddress = new Uri(kbankUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(req.Header.TokenType, req.Header.AccessToken);
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };

                var bodyRequest = new JsonObject
                {
                    { "partnerTxnUid", req.Detail.PartnerTxnUid },
                    { "partnerId", req.Detail.PartnerId },
                    { "partnerSecret", Environment.GetEnvironmentVariable("PartnerSecret") },
                    { "requestDt", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzzz", CultureInfo.InvariantCulture) },
                    { "merchantId", req.Detail.MerchantId },
                    { "terminalId", req.Detail.TerminalId },
                    { "qrType", Environment.GetEnvironmentVariable("QRType") },
                    { "txnAmount", req.Detail.Amount },
                    { "txnCurrencyCode", "THB" },
                    { "reference1", req.Detail.Reference1 },
                    { "reference2", req.Detail.Reference2 },
                    { "reference3", req.Detail.Reference3 },
                    { "reference4", req.Detail.Reference4 },
                    { "metadata", req.Detail.Details }
                };

                // InsertLogTxnRequest
                LogTxnRequest txnReq = new()
                {
                    TxnDate = DateTime.Now,
                    TxnType = methodName,
                    PartnerTxnUid = req.Detail.PartnerTxnUid,
                    PartnerId = Environment.GetEnvironmentVariable("PartnerId")!,
                    Body = bodyRequest.ToJsonString()
                };
                await _logTxnRepository.InsertLogTxnRequest(txnReq);

                var responseMessage = await client.PostAsJsonAsync(endpoint, bodyRequest);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                // InsertLogTxnResponse
                LogTxnResponse txnRes = new()
                {
                    TxnDate = DateTime.Now,
                    TxnType = methodName,
                    PartnerTxnUid = req.Detail.PartnerTxnUid,
                    PartnerId = Environment.GetEnvironmentVariable("PartnerId")!,
                    StatusCode = responseMessage.StatusCode.ToString(),
                    Body = responseContent
                };
                await _logTxnRepository.InsertLogTxnResponse(txnRes);

                if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Message = responseContent
                    };
                }

                var response = JsonSerializer.Deserialize<QRCodePaymentResDto?>(responseContent);

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = response,
                    Message = AppConstant.STATUS_SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResultResponse> QRPaymentCancel(QRCodePaymentCancelReqDto req)
        {
            string methodName = GetCurrentAsyncMethod.GetName(MethodBase.GetCurrentMethod());

            try
            {
                string kbankUrl = Environment.GetEnvironmentVariable("KBanksUrl")!;
                string endpoint = "/v1/qrpayment/cancel";

                HttpClient client = new();
                client.BaseAddress = new Uri(kbankUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(req.Header.TokenType, req.Header.AccessToken);
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };

                var bodyRequest = new JsonObject
                {
                    { "partnerTxnUid", req.Detail.PartnerTxnUid },
                    { "partnerId", req.Detail.PartnerId },
                    { "partnerSecret", Environment.GetEnvironmentVariable("PartnerSecret") },
                    { "requestDt", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzzz", CultureInfo.InvariantCulture) },
                    { "merchantId", req.Detail.MerchantId },
                    { "terminalId", req.Detail.TerminalId },
                    { "origPartnerTxnUid", req.Detail.OriginalPartnerTxnUid },
                };

                // InsertLogTxnRequest
                LogTxnRequest txnReq = new()
                {
                    TxnDate = DateTime.Now,
                    TxnType = methodName,
                    PartnerTxnUid = req.Detail.PartnerTxnUid,
                    PartnerId = Environment.GetEnvironmentVariable("PartnerId")!,
                    Body = bodyRequest.ToJsonString()
                };
                await _logTxnRepository.InsertLogTxnRequest(txnReq);

                var responseMessage = await client.PostAsJsonAsync(endpoint, bodyRequest);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                // InsertLogTxnResponse
                LogTxnResponse txnRes = new()
                {
                    TxnDate = DateTime.Now,
                    TxnType = methodName,
                    PartnerTxnUid = req.Detail.PartnerTxnUid,
                    PartnerId = Environment.GetEnvironmentVariable("PartnerId")!,
                    StatusCode = responseMessage.StatusCode.ToString(),
                    Body = responseContent
                };
                await _logTxnRepository.InsertLogTxnResponse(txnRes);

                if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Message = responseContent
                    };
                }

                var response = JsonSerializer.Deserialize<QRCodePaymentCancelResDto?>(responseContent);

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = response,
                    Message = AppConstant.STATUS_SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResultResponse> QRPaymentInquiry(QRCodePaymentInquiryReqDto req)
        {
            string methodName = GetCurrentAsyncMethod.GetName(MethodBase.GetCurrentMethod());

            try
            {
                string kbankUrl = Environment.GetEnvironmentVariable("KBanksUrl")!;
                string endpoint = "/v1/qrpayment/v4/inquiry";

                HttpClient client = new();
                client.BaseAddress = new Uri(kbankUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(req.Header.TokenType, req.Header.AccessToken);
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };

                var bodyRequest = new JsonObject
                {
                    { "partnerTxnUid", req.Detail.PartnerTxnUid },
                    { "partnerId", req.Detail.PartnerId },
                    { "partnerSecret", Environment.GetEnvironmentVariable("PartnerSecret") },
                    { "requestDt", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzzz", CultureInfo.InvariantCulture) },
                    { "merchantId", req.Detail.MerchantId },
                    { "terminalId", req.Detail.TerminalId },
                    { "qrType", Environment.GetEnvironmentVariable("QRType") },
                    { "origPartnerTxnUid", req.Detail.OriginalPartnerTxnUid },
                    { "txnNo", req.Detail.TxnNo },
                };

                // InsertLogTxnRequest
                LogTxnRequest txnReq = new()
                {
                    TxnDate = DateTime.Now,
                    TxnType = methodName,
                    PartnerTxnUid = req.Detail.PartnerTxnUid,
                    PartnerId = Environment.GetEnvironmentVariable("PartnerId")!,
                    Body = bodyRequest.ToJsonString()
                };
                await _logTxnRepository.InsertLogTxnRequest(txnReq);

                var responseMessage = await client.PostAsJsonAsync(endpoint, bodyRequest);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                // InsertLogTxnResponse
                LogTxnResponse txnRes = new()
                {
                    TxnDate = DateTime.Now,
                    TxnType = methodName,
                    PartnerTxnUid = req.Detail.PartnerTxnUid,
                    PartnerId = Environment.GetEnvironmentVariable("PartnerId")!,
                    StatusCode = responseMessage.StatusCode.ToString(),
                    Body = responseContent
                };
                await _logTxnRepository.InsertLogTxnResponse(txnRes);

                if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Message = responseContent
                    };
                }

                var response = JsonSerializer.Deserialize<QRCodePaymentInquiryResDto?>(responseContent);

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = response,
                    Message = AppConstant.STATUS_SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResultResponse> QRPaymentSettlement(QRCodePaymentSettlemenReqDto req)
        {
            string methodName = GetCurrentAsyncMethod.GetName(MethodBase.GetCurrentMethod());

            try
            {
                string kbankUrl = Environment.GetEnvironmentVariable("KBanksUrl")!;
                string endpoint = "/v1/qrpayment/settlement";

                HttpClient client = new();
                client.BaseAddress = new Uri(kbankUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(req.Header.TokenType, req.Header.AccessToken);
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };

                var bodyRequest = new JsonObject
                {
                    { "partnerTxnUid", req.Detail.PartnerTxnUid },
                    { "partnerId", req.Detail.PartnerId },
                    { "partnerSecret", Environment.GetEnvironmentVariable("PartnerSecret") },
                    { "requestDt", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzzz", CultureInfo.InvariantCulture) },
                    { "merchantId", req.Detail.MerchantId },
                    { "terminalId", req.Detail.TerminalId },
                    { "qrType", Environment.GetEnvironmentVariable("QRType") }
                };

                // InsertLogTxnRequest
                LogTxnRequest txnReq = new()
                {
                    TxnDate = DateTime.Now,
                    TxnType = methodName,
                    PartnerTxnUid = req.Detail.PartnerTxnUid,
                    PartnerId = Environment.GetEnvironmentVariable("PartnerId")!,
                    Body = bodyRequest.ToJsonString()
                };
                await _logTxnRepository.InsertLogTxnRequest(txnReq);

                var responseMessage = await client.PostAsJsonAsync(endpoint, bodyRequest);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                // InsertLogTxnResponse
                LogTxnResponse txnRes = new()
                {
                    TxnDate = DateTime.Now,
                    TxnType = methodName,
                    PartnerTxnUid = req.Detail.PartnerTxnUid,
                    PartnerId = Environment.GetEnvironmentVariable("PartnerId")!,
                    StatusCode = responseMessage.StatusCode.ToString(),
                    Body = responseContent
                };
                await _logTxnRepository.InsertLogTxnResponse(txnRes);

                if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Message = responseContent
                    };
                }

                var response = JsonSerializer.Deserialize<QRPaymentSettlementResDto>(responseContent);

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = response,
                    Message = AppConstant.STATUS_SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResultResponse> QRPaymentVoid(QRCodePaymentVoidReqDto req)
        {
            string methodName = GetCurrentAsyncMethod.GetName(MethodBase.GetCurrentMethod());

            try
            {
                string kbankUrl = Environment.GetEnvironmentVariable("KBanksUrl")!;
                string endpoint = "v1/qrpayment/void";

                HttpClient client = new();
                client.BaseAddress = new Uri(kbankUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(req.Header.TokenType, req.Header.AccessToken);
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };

                var bodyRequest = new JsonObject
                {
                    { "partnerTxnUid", req.Detail.PartnerTxnUid },
                    { "partnerId", req.Detail.PartnerId },
                    { "partnerSecret", Environment.GetEnvironmentVariable("PartnerSecret") },
                    { "requestDt", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzzz", CultureInfo.InvariantCulture) },
                    { "merchantId", req.Detail.MerchantId },
                    { "terminalId", req.Detail.TerminalId },
                    { "origPartnerTxnUid", req.Detail.OrigPartnerTxnUid},
                    { "txnNo", req.Detail.TxnNo},
                };

                // InsertLogTxnRequest
                LogTxnRequest txnReq = new()
                {
                    TxnDate = DateTime.Now,
                    TxnType = methodName,
                    PartnerTxnUid = req.Detail.PartnerTxnUid,
                    PartnerId = Environment.GetEnvironmentVariable("PartnerId")!,
                    Body = bodyRequest.ToJsonString()
                };
                await _logTxnRepository.InsertLogTxnRequest(txnReq);

                var responseMessage = await client.PostAsJsonAsync(endpoint, bodyRequest);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                // InsertLogTxnResponse
                LogTxnResponse txnRes = new()
                {
                    TxnDate = DateTime.Now,
                    TxnType = methodName,
                    PartnerTxnUid = req.Detail.PartnerTxnUid,
                    PartnerId = Environment.GetEnvironmentVariable("PartnerId")!,
                    StatusCode = responseMessage.StatusCode.ToString(),
                    Body = responseContent
                };
                await _logTxnRepository.InsertLogTxnResponse(txnRes);

                if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Message = responseContent
                    };
                }

                var response = JsonSerializer.Deserialize<QRPaymentVoidResDto>(responseContent);

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = response,
                    Message = AppConstant.STATUS_SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResultResponse> QRPaymentCallBack(QRPaymentCallBackReqDto req)
        {
            string methodName = GetCurrentAsyncMethod.GetName(MethodBase.GetCurrentMethod());

            try
            {
                // InsertLogTxnRequest
                LogTxnRequest txnReq = new()
                {
                    TxnDate = DateTime.Now,
                    TxnType = methodName,
                    PartnerTxnUid = req.PartnerTxnUid,
                    PartnerId = req.PartnerId,
                    Body = JsonSerializer.Serialize(req)
                };
                await _logTxnRepository.InsertLogTxnRequest(txnReq);

                QRPaymentCallBackResDto res = new();
                res.StatusCode = req.StatusCode;
                res.ErrorCode = req.ErrorCode;
                res.ErrorDesc = req.ErrorDesc;

                // InsertLogTxnResponse
                LogTxnResponse txnRes = new()
                {
                    TxnDate = DateTime.Now,
                    TxnType = methodName,
                    PartnerTxnUid = req.PartnerTxnUid,
                    PartnerId = req.PartnerId,
                    StatusCode = req.StatusCode,
                    Body = JsonSerializer.Serialize(res)
                };
                await _logTxnRepository.InsertLogTxnResponse(txnRes);

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = res,
                    Message = AppConstant.STATUS_SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
