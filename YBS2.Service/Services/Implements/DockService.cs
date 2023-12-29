using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services.Implements
{
    public class DockService : IDockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DockService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public Task<DockDto?> Create(DockInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<DockListingDto>> GetAll(DockPageRequest pageRequest)
        {
            List<DockListingDto> list = await _unitOfWork.DockRepository
                                            .GetAll()
                                            .Select(dock => _mapper.Map<DockListingDto>(dock))
                                            .ToListAsync();
            return new DefaultPageResponse<DockListingDto>
            {
                Data = list,
                PageCount = 0,
                PageIndex = 0,
                PageSize = 0,
                TotalItem = list.Count
            };
        }

        public async Task<DockDto?> GetDetails(Guid id)
        {
            return _mapper.Map<DockDto>(
                await _unitOfWork.DockRepository.GetByID(id)
            );
        }

        public Task<DockDto?> Update(Guid id, DockInputDto inputDto)
        {
            throw new NotImplementedException();
        }
    }
}