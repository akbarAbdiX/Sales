using System.Collections.Generic;
using BigBang.WebServer.Common.Exceptions;

namespace BigBang.App.Sales.Common.Exceptions
{
    public static class Exceptions
    {
        private static readonly Dictionary<ExceptionCode, ExceptionDetail> ExceptionsRegistry =
           new Dictionary<ExceptionCode, ExceptionDetail>
           {
                #region Document 1XXX
               {
                    ExceptionCode.DocumentNumberOrDateIsInvalid,
                    new ExceptionDetail
                    {
                        Code = ExceptionCode.DocumentNumberOrDateIsInvalid,
                        Message = "شماره یا زمان فاکتور باید از فاکتور قبلی جدیدتر باشد.",
                        Category = ExceptionCategories.Document
                    }
               },

               {
                     ExceptionCode.DocumentNumberIsDuplicate,
                    new ExceptionDetail
                    {
                        Code = ExceptionCode.DocumentNumberIsDuplicate,
                        Message = "فاکتوری با همین شماره و زمان صدور قبلاً ثبت شده است.",
                        Category = ExceptionCategories.Document
                    }
               },

               {
                     ExceptionCode.DocumentStateIsInvalid,
                    new ExceptionDetail
                    {
                        Code = ExceptionCode.DocumentStateIsInvalid,
                        Message ="امکان تغییر جزئبات در وضعیتی بغیر از ثبت اولیه وجود ندارد ",
                        Category = ExceptionCategories.Document
                    }
               },
               {
                   ExceptionCode.LastDocumentIsNotConfiermd,
                   new ExceptionDetail
                   {
                       Code = ExceptionCode.LastDocumentIsNotConfiermd,
                       Message = "فاکتور قبلی در مرحله ثبت اولیه است",
                       Category = ExceptionCategories.Document
                   }
               },
               #endregion

               #region ProductAndServiceRate 3XXX
                {
                    ExceptionCode.ProductAndServiceRateDateWasExpier,
                    new ExceptionDetail
                    {
                        Code = ExceptionCode.ProductAndServiceRateDateWasExpier,
                        Message = "نرخ کالا/خدمت در تاریخ سند معتبر نیست.",
                        Category = ExceptionCategories.ProductAndServiceRate
                    }
                },
                {
                    ExceptionCode.ProductAndServiceRateNotValidOnDocumentDate,
                    new ExceptionDetail
                    {
                        Code = ExceptionCode.ProductAndServiceRateNotValidOnDocumentDate,
                        Message = "برای این کالا/خدمت در تاریخ سند نرخ معتبری تعریف نشده است.",
                        Category = ExceptionCategories.ProductAndServiceRate
                    }
                }
               #endregion
           };
        public static BigBangException Raise(ExceptionCode code, object exceptionData, params object[] messageArgs)
        {
            var exceptionDetail = ExceptionsRegistry.TryGetValue(code, out var error) ? error : GetUnknownError(code);
            return BigBangException.New()
                .WithCode(((int)code).ToString())
                .WithMessage(exceptionDetail.Message)
                .WithErrorData(exceptionData)
                .WithMessageArgs(messageArgs)
                .Build();
        }

        public static BigBangException Raise(ExceptionCode code, object exceptionData)
        {
            var exceptionDetail = ExceptionsRegistry.TryGetValue(code, out var error) ? error : GetUnknownError(code);
            return BigBangException.New()
                .WithCode(((int)code).ToString())
                .WithMessage(exceptionDetail.Message)
                .WithErrorData(exceptionData)
                .Build();
        }

        public static BigBangException Raise(ExceptionCode code)
        {
            var exceptionDetail = ExceptionsRegistry.TryGetValue(code, out var error) ? error : GetUnknownError(code);
            return BigBangException.New()
                .WithCode(((int)code).ToString())
                .WithMessage(exceptionDetail.Message)
                .Build();
        }

        private static ExceptionDetail GetUnknownError(ExceptionCode code)
        {
            return new ExceptionDetail
            {
                Code = code,
                Message = "An unknown error occurred",
                Category = ExceptionCategories.Unknown
            };
        }
    }
}