﻿using AutoMapper;
using Inlamningsuppgift_Marie.Data;
using Inlamningsuppgift_Marie.Data.Entities;
using Inlamningsuppgift_Marie.Models.Album;
using Microsoft.EntityFrameworkCore;

namespace Inlamningsuppgift_Marie.Services
{
    public interface IAlbumService
    {
        public Task<CreateAlbumModel> CreateAlbumAsync(CreateAlbumModel request);
        public Task<IEnumerable<Album>> GetAllAlbumsAsync();
        public Task<bool> DeleteAlbumAsync(int id);
    }








    public class AlbumService : IAlbumService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public AlbumService(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public async Task<CreateAlbumModel> CreateAlbumAsync(CreateAlbumModel request)
        {
            if (!await _databaseContext.Albums.AnyAsync(x => x.AlbumName == request.AlbumName))
            {
                var albumEntity = _mapper.Map<AlbumEntity>(request);

                await _databaseContext.AddAsync(albumEntity);
                await _databaseContext.SaveChangesAsync();
                return _mapper.Map<CreateAlbumModel>(albumEntity);
            }

            return null;

        }

        public async Task<bool> DeleteAlbumAsync(int id)
        {
            var albumEntity = await _databaseContext.Albums.FindAsync(id);
            if (albumEntity != null)
            {
                _databaseContext.Remove(albumEntity);
                await _databaseContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {
            return _mapper.Map<IEnumerable<Album>>(await _databaseContext.Albums.Include(x => x.Artist).ToListAsync());

            // .Include(x => x.Artist)


            // returnera en model på det som skall vara med istället kanske?
            /* FUNKAR UTAN MAPPER
            var albums = new List<Album>();
            foreach (var item in await _databaseContext.Albums.Include(x => x.Artist).ToListAsync())
                albums.Add(new Album
                {
                    Id = item.AlbumId,
                    AlbumName = item.AlbumName,
                    ArtistId = item.ArtistId,
                    ArtistName = item.Artist.ArtistName
                });

            return albums;*/
        }
    }
}