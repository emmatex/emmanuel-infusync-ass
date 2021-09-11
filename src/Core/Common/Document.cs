using Core.Interfaces;
using MongoDB.Bson;
using System;

namespace Core.Common
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt => Id.CreationTime;
    }
}
