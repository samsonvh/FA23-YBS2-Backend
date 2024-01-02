using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YBS2.Data.Enums;
using YBS2.Data.Models;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;
using YBS2.Service.Exceptions;

namespace YBS2.Service.Services.Implements
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public TourService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public async Task<TourDto?> Create(TourInputDto inputDto, ClaimsPrincipal claims)
        {
            Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
            Yacht? existingYacht = await _unitOfWork.YachtRepository
                .Find(yacht => yacht.Id == inputDto.YachtId)
                .FirstOrDefaultAsync();
            if (existingYacht == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.YachtId = $"Yacht with Id {inputDto.YachtId} does not exist.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.YachtId, Errors);
            }
            Tour tour = _mapper.Map<Tour>(inputDto);
            tour.MaximumGuest = existingYacht.TotalPassenger;
            tour.Status = EnumTourStatus.Active;
            tour.CompanyId = companyId;
            _unitOfWork.TourRepository.Add(tour);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TourDto>(tour);
        }

        public Task<TourDto?> Create(TourInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<TourListingDto>> GetAll(TourPageRequest pageRequest)
        {
            List<TourListingDto> list = await _unitOfWork.TourRepository.GetAll()
                                                                        .Select(tour => _mapper.Map<TourListingDto>(tour))
                                                                        .ToListAsync();
            int totalItem = list.Count();
            return new DefaultPageResponse<TourListingDto>
            {
                Data = list,
                PageCount = 0,
                PageIndex = 0,
                PageSize = 0,
                TotalItem = totalItem
            };
        }

        public async Task<TourDto?> GetDetails(Guid id)
        {
            return await _unitOfWork.TourRepository.Find(tour => tour.Id == id)
                                                    .Select(tour => _mapper.Map<TourDto>(tour))
                                                    .FirstOrDefaultAsync();
        }

        public Task<TourDto?> Update(Guid id, TourInputDto inputDto)
        {
            throw new NotImplementedException();
        }
    }
}