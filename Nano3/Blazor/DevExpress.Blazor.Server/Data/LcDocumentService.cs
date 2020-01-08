using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DevExpress.Blazor.Server.Data
{
    public class LcDocumentService : ILcDocumentService
    {
        private readonly AbsCoreContext _context;
        private readonly IConfiguration _configuration;

        public LcDocumentService(AbsCoreContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Task<List<LcDocumentList>> GetLcDocumentsAsync()
        {
            return _context.LcDocuments.ToListAsync();
        }


        public async Task<List<QuotationHistoryByPartnerDto>> GetQuotationHistoryByPartner(int partnerId)
        {
            var sql = @"SELECT   SUM(D.[Qty]) AS [Qty] ,
									[D].[PartnerId] ,
									[D].[QuotationDate] ,
									[D].[EnquiryDate] ,
									[D].[Name] AS [Reference] ,
									[P].[Name] AS [Customer] ,
									[SP].[Name] AS [SalesPerson] ,
									[PC].[Name] AS [Category] ,
									[D].[Unit] ,
									[D].[UnitPrice],
									[D].[QuotationState] as [State]
						FROM     (   SELECT   TOP ( 100 ) [QL].[ProductCategoryId] ,
															[QL].[Qty] ,
															[QL].[Unit] ,
															[QL].[UnitPrice] ,
															[Q].[PartnerId] ,
															[Q].[SalesPersonId] ,
															[Q].[QuotationDate] ,
															[Q].[EnquiryDate] ,
															[Q].[Name] ,
															[Q].[QuotationState]
										FROM     [dbo].[QuotationLines] AS QL
												INNER JOIN [dbo].[Quotations] AS Q ON Q.[Id] = QL.[QuotationId]
										WHERE    ( Q.[PartnerId] = @PartnerId )
												AND ( NOT ( Q.[Canceled] = 1 ))
										ORDER BY Q.[QuotationDate] DESC ) AS D
									INNER JOIN [dbo].[Partners] AS P ON P.[Id] = D.[PartnerId]
									INNER JOIN [dbo].[SalesPersons] AS SP ON SP.[Id] = D.[SalesPersonId]
									LEFT OUTER JOIN [dbo].[ProductCategories] AS PC ON PC.[Id] = D.[ProductCategoryId]
						GROUP BY [D].[PartnerId] ,
									[D].[QuotationDate] ,
									[D].[EnquiryDate] ,
									[D].[Name] ,
									[P].[Name] ,
									[SP].[Name] ,
									[PC].[Name] ,
									[D].[Unit] ,
									[D].[UnitPrice],
									[D].[QuotationState];";
            using (var con = new SqlConnection(_configuration.GetConnectionString("AbsCore")))
            {
                con.Open();
                return (await con.QueryAsync<QuotationHistoryByPartnerDto>(sql, new { PartnerId = partnerId }, commandType: CommandType.Text)).ToList();
            }
        }
    }


    public class QuotationHistoryByPartnerDto
    {
        public int PartnerId { get; set; }
        public string Customer { get; set; }
        public string Reference { get; set; }
        public DateTime? EnquiryDate { get; set; }
        public DateTime QuotationDate { get; set; }
        public string SalesPerson { get; set; }
        public string Category { get; set; }
        public decimal Qty { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }

        public int State { get; set; }
    }

}
