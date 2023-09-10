using Back_End_Layihe.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.Entities
{
    public class EventSpeakers : BaseEntity
    {
        public string SpeakersPhoto { get; set; }
        public string SpeakersName { get; set; }
        public string SpeakersWork { get; set; }
    }
}
