using Hymap.Application.DTOs;
using Hymap.Application.Interfaces;
using Hymap.Domain.Entities;

namespace Hymap.Application.Services
{
    public class RuteService
    {
        private readonly IRuteRepository _ruteRepo;
        private readonly IWilayahRepository _wilayahRepo;

        public RuteService(IRuteRepository ruteRepo, IWilayahRepository wilayahRepo)
        {
            _ruteRepo = ruteRepo;
            _wilayahRepo = wilayahRepo;
        }

        public async Task<List<DataRuteItemDto>> GetListAsync(bool isActive, string searchQuery = "")
        {
            var allRutes = await _ruteRepo.GetAllAsync(null);
            var allWilayahs = await _wilayahRepo.GetAllAsync(null);

            var result = new List<DataRuteItemDto>();

            // To mimic the UI, group wilayahs under their rute
            foreach (var d in allRutes.OrderBy(x => x.Code))
            {
                var wilayahsInRute = allWilayahs.Where(x => x.RuteId == d.Id).OrderBy(x => x.Code).ToList();
                
                bool matchSearchRute = string.IsNullOrEmpty(searchQuery) || d.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) || d.Code.Contains(searchQuery, StringComparison.OrdinalIgnoreCase);

                var matchingWilayahs = new List<Wilayah>();
                foreach (var b in wilayahsInRute)
                {
                    bool matchSearchWilayah = string.IsNullOrEmpty(searchQuery) || b.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) || b.Code.Contains(searchQuery, StringComparison.OrdinalIgnoreCase);
                    
                    // A wilayah matches if it matches the search directly, OR if its parent rute matched the search
                    bool isWilayahMatch = matchSearchWilayah || matchSearchRute;

                    if (b.IsActive == isActive && isWilayahMatch)
                    {
                        matchingWilayahs.Add(b);
                    }
                }

                // Show Rute if it matches search AND its status matches, OR if it has ANY matching wilayahs
                bool showRute = (d.IsActive == isActive && matchSearchRute) || matchingWilayahs.Any();

                if (showRute)
                {
                    result.Add(new DataRuteItemDto
                    {
                        Id = d.Id,
                        Code = d.Code,
                        Name = d.Name,
                        IsRute = true,
                        IsActive = d.IsActive
                    });
                }

                // Then add all matching wilayahs
                foreach (var b in matchingWilayahs)
                {
                    result.Add(new DataRuteItemDto
                    {
                        Id = b.Id,
                        Code = b.Code,
                        Name = b.Name,
                        IsRute = false,
                        IsActive = b.IsActive,
                        ParentRuteId = d.Id
                    });
                }
            }

            return result;
        }

        public async Task<List<Rute>> GetActiveRutesForDropdownAsync()
        {
            return await _ruteRepo.GetAllAsync(true);
        }

        public async Task AddRuteAsync(string name)
        {
            var count = await _ruteRepo.GetCountAsync();
            var code = $"RUT-{(count + 1):D3}";
            await _ruteRepo.AddAsync(new Rute { Code = code, Name = name, IsActive = true });
        }

        public async Task AddWilayahAsync(string name, int ruteId)
        {
            var count = await _wilayahRepo.GetCountAsync();
            var code = $"WIL-{(count + 1):D3}";
            await _wilayahRepo.AddAsync(new Wilayah { Code = code, Name = name, RuteId = ruteId, IsActive = true });
        }

        public async Task UpdateStatusAsync(int id, bool isRute, bool isActive)
        {
            if (isRute)
            {
                var d = await _ruteRepo.GetByIdAsync(id);
                if (d != null)
                {
                    d.IsActive = isActive;
                    await _ruteRepo.UpdateAsync(d);

                    // Cascade deactivation: Jika rute dinonaktifkan, nonaktifkan semua wilayahnya
                    if (!isActive)
                    {
                        var allWilayahs = await _wilayahRepo.GetAllAsync(null);
                        var childWilayahs = allWilayahs.Where(b => b.RuteId == id && b.IsActive).ToList();
                        foreach (var b in childWilayahs)
                        {
                            b.IsActive = false;
                            await _wilayahRepo.UpdateAsync(b);
                        }
                    }
                }
            }
            else
            {
                var b = await _wilayahRepo.GetByIdAsync(id);
                if (b != null)
                {
                    b.IsActive = isActive;
                    await _wilayahRepo.UpdateAsync(b);
                }
            }
        }

        public async Task UpdateRuteAsync(int id, string newName, bool isActive)
        {
            var d = await _ruteRepo.GetByIdAsync(id);
            if (d != null)
            {
                d.Name = newName;
                d.IsActive = isActive;
                await _ruteRepo.UpdateAsync(d);

                // Cascade deactivation: Jika rute dinonaktifkan, nonaktifkan semua wilayahnya
                if (!isActive)
                {
                    var allWilayahs = await _wilayahRepo.GetAllAsync(null);
                    var childWilayahs = allWilayahs.Where(b => b.RuteId == id && b.IsActive).ToList();
                    foreach (var b in childWilayahs)
                    {
                        b.IsActive = false;
                        await _wilayahRepo.UpdateAsync(b);
                    }
                }
            }
        }

        public async Task UpdateWilayahAsync(int id, string newName, int newRuteId, bool isActive)
        {
            var b = await _wilayahRepo.GetByIdAsync(id);
            if (b != null)
            {
                b.Name = newName;
                b.RuteId = newRuteId;
                b.IsActive = isActive;
                await _wilayahRepo.UpdateAsync(b);
            }
        }

        public async Task DeleteAsync(int id, bool isRute)
        {
            if (isRute)
            {
                var d = await _ruteRepo.GetByIdAsync(id);
                if (d != null && !d.IsActive) await _ruteRepo.DeleteAsync(d);
            }
            else
            {
                var b = await _wilayahRepo.GetByIdAsync(id);
                if (b != null && !b.IsActive) await _wilayahRepo.DeleteAsync(b);
            }
        }
    }
}
