using Core.Interfaces;
using MongoDB.Bson;
using System;

namespace Core.Common
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedOn => Id.CreationTime;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
