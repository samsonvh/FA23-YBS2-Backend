using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YBS.Service.Utils;
using YBS2.Data.Enums;
using YBS2.Data.Models;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services.Implements
{
    public class MemberService : IMemberService
    {
        public Task<bool> ChangeStatus(Guid id, string name)
        {
            throw new NotImplementedException();
        }

        public Task<MemberDto?> Create(MemberInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id, string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<MemberListingDto>> GetAll(MemberPageRequest pageRequest)
        {
            throw new NotImplementedException();
        }

        public Task<MemberDto?> GetDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<MemberDto?> Update(Guid id, MemberInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        private IQueryable<Member> FilterGetAll(IQueryable<Member> query, MemberPageRequest pageRequest)
        {
            if (pageRequest.FullName != null)
            {
                query = query.Where(member => member.FullName.Trim().ToLower().Contains(pageRequest.FullName.ToLower()));
            }

            if (pageRequest.PhoneNumber != null)
            {
                query = query.Where(member => member.PhoneNumber.Contains(pageRequest.PhoneNumber));
            }

            if (pageRequest.Status != null && Enum.IsDefined(typeof(EnumMemberStatus), pageRequest.Status))
            {
                query = query.Where(member => member.Status == pageRequest.Status);
            }
            query = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                    ? query.SortBy(pageRequest.OrderBy, pageRequest.IsDescending)
                    : pageRequest.IsDescending
                    ? query.OrderBy(member => member.Id)
                    : query.OrderByDescending(member => member.Id);
            return query;
        }
    }
}