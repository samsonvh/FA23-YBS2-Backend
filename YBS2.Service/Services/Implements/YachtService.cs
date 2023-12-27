using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YBS2.Data.Models;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services.Implements
{
    public class YachtService : IYachtService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public YachtService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public Task<YachtDto?> Create(YachtInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<YachtListingDto>> GetAll(YachtPageRequest pageRequest)
        {
            List<YachtListingDto> list = await _unitOfWork.YachtRepository.GetAll().Select(yacht => _mapper.Map<YachtListingDto>(yacht)).ToListAsync();
            return new DefaultPageResponse<YachtListingDto>
            {
                Data = list,
                PageCount = 0,
                PageIndex = 0,
                PageSize = 0,
                TotalItem = list.Count
            };
        }

        public async Task<YachtDto?> GetDetails(Guid id)
        {
            return _mapper.Map<YachtDto>
            (await _unitOfWork.YachtRepository.GetByID(id));
        }

        public Task<YachtDto?> Update(Guid id, YachtInputDto inputDto)
        {
            throw new NotImplementedException();
        }
    }
}