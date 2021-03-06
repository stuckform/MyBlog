﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Services
{
    public class BasicImageService : IImageService

    {
       public async Task<byte[]> EncodeFileAsync(IFormFile formFile)
        {
            if (formFile == null)
                return null;
           
            using var ms = new MemoryStream();
            //Copy image into memorystream
            await formFile.CopyToAsync(ms);           
            return ms.ToArray();


        }

        public string DecodeFile(byte[] imageData, string contentType)
        {
            if (imageData == null)
                return null;

            var imageArray = Convert.ToBase64String(imageData);
            return $"data:{contentType};base64,{imageArray}";
        }

        public string RecordContentType(IFormFile formFile)
        {
            if (formFile == null)
                return null;

            return formFile.ContentType;
        }




    }

}                
