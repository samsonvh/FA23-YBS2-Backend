using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS2.Data.Models;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;
using YBS2.Service.Exceptions;

namespace YBS2.Service.Services.Implements
{
    public class UpdateRequestService : IUpdateRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _storageService;
        private readonly IMapper _mapper;

        public UpdateRequestService(IUnitOfWork unitOfWork, IFirebaseStorageService storageService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _mapper = mapper;
        }

        public Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateRequestDto?> Create(UpdateRequestInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateRequestDto?> Create(UpdateRequestInputDto inputDto, Guid companyId)
        {
            UpdateRequest updateRequest = _mapper.Map<UpdateRequest>(inputDto);
            _unitOfWork.UpdateRequestRepository.Add(_mapper.Map<UpdateRequest>(inputDto));
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UpdateRequestDto>(updateRequest);
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<UpdateRequestListingDto>> GetAll(UpdateRequestPageRequest pageRequest)
        {
            List<UpdateRequestListingDto> list = await _unitOfWork.UpdateRequestRepository
                .GetAll()
                .Select(updateRequest => _mapper.Map<UpdateRequestListingDto>(updateRequest))
                .ToListAsync();
            return new DefaultPageResponse<UpdateRequestListingDto>
            {
                Data = list,
                TotalItem = list.Count,
                PageCount = list.Count / pageRequest.PageSize,
                PageSize = pageRequest.PageSize,
                PageIndex = pageRequest.PageIndex
            };
        }

        public Task<UpdateRequestDto?> GetDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateRequestDto?> Update(Guid id, UpdateRequestInputDto inputDto)
        {
            throw new NotImplementedException();
        }
    }
}
