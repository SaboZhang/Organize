﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTidy.Model;

[Table("Schedule")]
public class ScheduleTable
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public string Minutes { get; set; } = "00";

    public string Hours { get; set; } = "00";

    public string WeeklyDayNumber { get; set; } = "";

    public string DailyInMonthNumber { get; set; } = "";

    public string Monthly { get; set; } = "";

    public string CronExpression { get; set; } = "";
}
