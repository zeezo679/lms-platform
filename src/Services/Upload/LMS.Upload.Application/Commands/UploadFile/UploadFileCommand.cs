using LMS.Upload.Domain.Enums;
using Microsoft.AspNetCore.Http;   
using MediatR;

namespace LMS.Upload.Application.Commands.UploadFile;

public record  UploadFileCommand(IFormFile File, UploadContext Context, Guid UploadedBy) 
: IRequest<string>;
