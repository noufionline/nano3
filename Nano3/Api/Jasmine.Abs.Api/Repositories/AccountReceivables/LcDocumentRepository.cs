using AutoMapper;
using AutoMapper.QueryableExtensions;
using Jasmine.Abs.Api.Dto;
using Jasmine.Abs.Api.Repositories.Contracts;
using Jasmine.Abs.Entities.Models.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Dapper;
using Jasmine.Abs.Entities;
using Jasmine.Abs.Api.Dto.AccountReceivables;

namespace Jasmine.Abs.Api.Repositories.AccountReceivables
{
    public class LcDocumentRepository : Repository<LcDocument, LcDocumentDto, LcDocumentListDto>, ILcDocumentRepository
    {

        readonly AbsContext _context;
        readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public LcDocumentRepository(AbsContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            ILogger<LcDocumentRepository> logger) : base(context, mapper, httpContextAccessor, logger)
        {
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
        }

        public async Task<LcDocumentForUpdateDto> GetLcDocumentForUpdateAsync(int id)
        {
            return await _context.LcDocuments
                .ProjectTo<LcDocumentForUpdateDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x=> x.Id==id).ConfigureAwait(false);
        }

        protected async override Task<List<LcDocumentListDto>> GetEntitiesAsync()
        {
            return await _context.LcDocuments.ProjectTo<LcDocumentListDto>(_mapper.ConfigurationProvider).ToListAsync().ConfigureAwait(false);
        }

        protected async override Task<LcDocumentDto> GetEntityAsync(int id)
        {
             return await _context.LcDocuments
                .ProjectTo<LcDocumentDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x=> x.Id==id).ConfigureAwait(false);
        }
        public async Task<LcDocumentDetailDto> GetLcDocumentDetailById(int id)
        {
            return await _context.LcDocuments
                .ProjectTo<LcDocumentDetailDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
        }

        public async Task<LcDocumentForPrintDto> GetLcDocumentForPrintAsync(int id)
        {
             return await _context.LcDocuments.Where(x=> x.Id==id)
                .ProjectTo<LcDocumentForPrintDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async  Task<List<CommercialInvoiceLineForPrintDto>> GetInvoicesAsync(int id)
        {
           using(var con=new SqlConnection(_configuration.GetConnectionString("CICONABS")))
           {
                con.Open();
                return (await con.QueryAsync<CommercialInvoiceLineForPrintDto>(@"SELECT    CI.InvoiceNo ,
                                                                                           CI.InvoiceDate ,
                                                                                           CI.Amount ,
                                                                                           CI.DueDate ,
                                                                                           CI.CommercialInvoiceStatus ,
                                                                                           DateOfSignature = DOS.Date ,
                                                                                           CI.LcDocumentId
                                                                                        FROM   dbo.CommercialInvoices AS CI
                                                                                        LEFT OUTER JOIN ( SELECT 
                                                                                                            CITH.InvoiceId ,
                                                                                                            Date
                                                                                                            FROM   dbo.CommercialInvoiceTransactionHistory AS CITH
                                                                                                            WHERE  CITH.CommercialInvoiceStatus = @StatusId 
                                                                                                        ) AS DOS ON DOS.InvoiceId = CI.Id
                                                                                WHERE CI.LcDocumentId=@DocumentId",new {DocumentId=id,StatusId=CommercialInvoiceStatusTypes.Signed})).ToList();
           }
        }
    }
}
