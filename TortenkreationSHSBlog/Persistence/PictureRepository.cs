﻿namespace TortenkreationSHSBlog.Persistence {

    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TortenkreationSHSBlog.Models;

    public class PictureRepository : IPictureRepository {

        private readonly DataContext dataContext;

        public PictureRepository(DataContext dataContext) {
            this.dataContext = dataContext;
        }

        public async Task Add(Picture picture) {
            await this.dataContext.AddAsync(picture);
            await this.dataContext.SaveChangesAsync();
            return;
        }

        public async Task Delete(Picture picture) {
            this.dataContext.Remove(picture);
            await this.dataContext.SaveChangesAsync();
            return;
        }

        public async Task<IEnumerable<Picture>> GetAll(bool showUnpublished, Occasion? occasion) {
            IEnumerable<Picture> pictures;
            if (null != occasion) {
                pictures = await this.dataContext.Pictures.Where(p => p.Occasion == occasion).OrderByDescending(p => p.CreatedAt).ToListAsync();
            } else {
                pictures = await this.dataContext.Pictures.OrderByDescending(p => p.CreatedAt).ToListAsync();
            }
            if (!showUnpublished) {
                pictures = pictures.Where(p => p.IsPublished == true).OrderByDescending(p => p.CreatedAt).ToList();
            }
            return pictures;
        }

        public async Task<Picture> GetById(int id) {
            var picture = await this.dataContext.Pictures.SingleOrDefaultAsync(p => p.Id == id);
            return picture;
        }

        public async Task<Picture> GetByUrl(string url) {
            var picture = await this.dataContext.Pictures.SingleOrDefaultAsync(p => p.GetUrl() == url);
            return picture;
        }

        public async Task<bool> IsTitleExisting(string title) {
            var exists = await this.dataContext.Pictures.AnyAsync(p => p.Title == title);
            return exists;
        }

        public async Task Update(Picture picture) {
            this.dataContext.Pictures.Update(picture);
            await this.dataContext.SaveChangesAsync();
            return;
        }

    }

}