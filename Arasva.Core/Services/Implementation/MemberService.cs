using Arasva.Core.DTO.Create;
using Arasva.Core.DTO.Response;
using Arasva.Core.DTO.Update;
using Arasva.Core.Interface;
using Arasva.Core.Models;
using Arasva.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Core.Services.Implementation
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repo;

        public MemberService(IMemberRepository repo)
        {
            _repo = repo;
        }

        public async Task<GlobalResponse<IEnumerable<MemberFullResponseDTO>>> GetAllAsync()
        {
            var members = await _repo.GetAllAsync();

            return new GlobalResponse<IEnumerable<MemberFullResponseDTO>>
            {
                success = true,
                message = string.Format(AppConstants.ActionSuccess),
                error = null,
                data = members.Select(m => new MemberFullResponseDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    IsActive = m.IsActive,
                    CreatedBy = m.CreatedBy,
                    CreatedDate = m.CreatedDate,
                    ModifiedBy = m.ModifiedBy,
                    ModifiedDate = m.ModifiedDate
                }),
                totalcount = members.Count()
            };
        }

        public async Task<GlobalResponse<MemberFullResponseDTO?>> GetByIdAsync(int id)
        {
            GlobalResponse<MemberFullResponseDTO?> globalResponse = new GlobalResponse<MemberFullResponseDTO?>();

            var m = await _repo.GetByIdAsync(id);
            if (m == null)
            {
                globalResponse.success = false;
                globalResponse.error = $"Member with ID {id} not found.";
                globalResponse.message = string.Empty;
                globalResponse.data = null;
                return globalResponse;
            }
            else
            {
                return new GlobalResponse<MemberFullResponseDTO?>
                {
                    success = true,
                    message = string.Format(AppConstants.ActionSuccess),
                    error = null,
                    data = new MemberFullResponseDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Email = m.Email,
                        IsActive = m.IsActive,
                        CreatedBy = m.CreatedBy,
                        CreatedDate = m.CreatedDate,
                        ModifiedBy = m.ModifiedBy,
                        ModifiedDate = m.ModifiedDate
                    },
                    totalcount = 1
                };
            }                
        }

        public async Task<GlobalResponse<MemberCreateResponseDTO>> CreateAsync(MemberCreateDTO dto)
        {
            try
            {
                var member = new Member
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    IsActive = dto.IsActive,
                    CreatedBy = dto.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                await _repo.AddAsync(member);
                await _repo.SaveAsync();

                var createdMember = new MemberCreateResponseDTO
                {
                    Id = member.Id,
                    Name = member.Name,
                    Email = member.Email,
                    IsActive = member.IsActive,
                    CreatedBy = member.CreatedBy,
                    CreatedDate = member.CreatedDate
                };

                return new GlobalResponse<MemberCreateResponseDTO>
                {
                    success = true,
                    message = string.Format(AppConstants.ActionSuccess),
                    error = null,
                    data = createdMember
                };
            }
            catch (Exception ex)
            {
                return new GlobalResponse<MemberCreateResponseDTO>
                {
                    success = false,
                    message = null,
                    error = string.Format(AppConstants.ErrorMessage, ex.Message),
                    data = null
                };
            }
        }

        public async Task<GlobalResponse<MemberUpdateResponseDTO?>> UpdateAsync(int id, MemberUpdateDTO dto)
        {
            try
            {
                var member = await _repo.GetByIdAsync(id);
                if (member == null) return null;

                member.Name = dto.Name;
                member.Email = dto.Email;
                member.IsActive = dto.IsActive;
                member.ModifiedBy = dto.ModifiedBy;
                member.ModifiedDate = DateTime.Now;

                _repo.Update(member);
                await _repo.SaveAsync();

                var updatedMember = new MemberUpdateResponseDTO
                {
                    Id = member.Id,
                    Name = member.Name,
                    Email = member.Email,
                    IsActive = member.IsActive,
                    ModifiedBy = member.ModifiedBy,
                    ModifiedDate = member.ModifiedDate
                };

                return new GlobalResponse<MemberUpdateResponseDTO?>
                {
                    success = true,
                    message = string.Format(AppConstants.ActionSuccess),
                    error = null,
                    data = updatedMember
                };
            }
            catch (Exception ex)
            {
                return new GlobalResponse<MemberUpdateResponseDTO?>
                {
                    success = false,
                    message = null,
                    error = string.Format(AppConstants.ErrorMessage, ex.Message),
                    data = null
                };
            }
        }
    }
}
