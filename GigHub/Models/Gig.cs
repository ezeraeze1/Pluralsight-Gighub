﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GigHub.Models
{
    public class Gig
    {
        public int Id { get; set; }

        public bool IsCanceled { get; private set; }

        public ApplicationUser Artist { get; set; }

        [Required]
        public string ArtistId { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        public Genre Genre { get; set; }

        [Required]
        public byte GenreId { get; set; }

        public ICollection<Attendance> Attenances { get; } = new Collection<Attendance>();

        public void Cancel()
        {
            IsCanceled = true;

            var notification = Notification.GigCanceled(this);

            foreach (var attendee in Attenances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }

        public void Update(string newVenue, DateTime newDateTime, byte genreId)
        {
            var notification = Notification.GigUpdated(this, DateTime, Venue);
            
            Venue = newVenue;
            DateTime = newDateTime;
            GenreId = genreId;

            foreach (var attendee in Attenances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }
    }
}