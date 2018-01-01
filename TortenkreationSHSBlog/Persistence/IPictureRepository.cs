﻿namespace TortenkreationSHSBlog.Persistence {

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TortenkreationSHSBlog.Models;

    public interface IPictureRepository {

        Task<IEnumerable<Picture>> GetAll();
        Task<Picture> GetByUrl(string pictureUrl);
        Task<Picture> GetById(int id);
        Task Add(Picture picture);
        Task<bool> IsTitleExisting(string title);
        Task Update(Picture picture);
        Task Delete(Picture picture);

    }

}