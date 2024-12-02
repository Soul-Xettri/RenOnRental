using MediatR;
using Microsoft.AspNetCore.Http;
using RevOnRental.Application.Interfaces;
using RevOnRental.Application.Services.Vehicles.Command;
using RevOnRental.Domain.Enums;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Businesses.Command
{
    public class AddBusinessDocumentCommand : IRequest<bool>
    {
        public int BusinessId { get; set; }
        public IFormFile NationalIdFront { get; set; }
        public IFormFile NationalIdBack { get; set; }
        public IFormFile Bluebook { get; set; }
        public IFormFile BusinessRegistrationDocument { get; set; }
    }

    public class AddBusinessDocumentCommandHandler : IRequestHandler<AddBusinessDocumentCommand, bool>
    {
        private readonly IAppDbContext _context;

        public AddBusinessDocumentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AddBusinessDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var businessDocuments = new List<BusinessDocument>();

                if (request.NationalIdFront != null)
                {
                    using var memoryStreamNationalIdFront = new MemoryStream();
                    await request.NationalIdFront.CopyToAsync(memoryStreamNationalIdFront);
                    var nationalIdFrontDocument = new BusinessDocument
                    {
                        BusinessId = request.BusinessId,
                        FileName = request.NationalIdFront.FileName,
                        ContentType = request.NationalIdFront.ContentType,
                        DocumentType = DocumentType.NationalIdFront,
                        FileContent = memoryStreamNationalIdFront.ToArray(),
                        UploadedDate = DateTime.Now,
                    };
                    businessDocuments.Add(nationalIdFrontDocument);
                }
                if (request.NationalIdBack != null)
                {
                    using var memoryStreamNationalIdBack = new MemoryStream();
                    await request.NationalIdBack.CopyToAsync(memoryStreamNationalIdBack);
                    var nationalIdBackDocument = new BusinessDocument
                    {
                        BusinessId = request.BusinessId,
                        FileName = request.NationalIdBack.FileName,
                        ContentType = request.NationalIdBack.ContentType,
                        DocumentType = DocumentType.NationalIdBack,
                        FileContent = memoryStreamNationalIdBack.ToArray(),
                        UploadedDate = DateTime.Now,
                    };
                    businessDocuments.Add(nationalIdBackDocument);
                }


                if (request.Bluebook != null)
                {
                    using var memoryStreamBluebook = new MemoryStream();
                    await request.Bluebook.CopyToAsync(memoryStreamBluebook);
                    var BluebookDocument = new BusinessDocument
                    {
                        BusinessId = request.BusinessId,
                        FileName = request.Bluebook.FileName,
                        ContentType = request.Bluebook.ContentType,
                        DocumentType = DocumentType.Bluebook,
                        FileContent = memoryStreamBluebook.ToArray(),
                        UploadedDate = DateTime.Now,
                    };
                    businessDocuments.Add(BluebookDocument);
                }

                if (request.BusinessRegistrationDocument != null)
                {
                    using var memoryStreamBusinessRegistrationDocument = new MemoryStream();
                    await request.BusinessRegistrationDocument.CopyToAsync(memoryStreamBusinessRegistrationDocument);
                    var BusinessRegistrationDocument = new BusinessDocument
                    {
                        BusinessId = request.BusinessId,
                        FileName = request.BusinessRegistrationDocument.FileName,
                        ContentType = request.BusinessRegistrationDocument.ContentType,
                        DocumentType = DocumentType.BusinessRegistrationDocument,
                        FileContent = memoryStreamBusinessRegistrationDocument.ToArray(),
                        UploadedDate = DateTime.Now,
                    };
                    businessDocuments.Add(BusinessRegistrationDocument);
                }

                if (businessDocuments.Count > 0)
                {
                    await _context.BusinesseDocuments.AddRangeAsync(businessDocuments);


                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
