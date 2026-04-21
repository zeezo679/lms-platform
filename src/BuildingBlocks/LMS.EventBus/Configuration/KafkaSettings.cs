using System;

namespace LMS.EventBus.Configuration;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = string.Empty; //broker address's
    public string GroupId { get; set; } = string.Empty;  //for load balancing and fault tolerance
    public string TopicPrefix { get; set; } = "lms.";
    public string[] Topics { get; set; } = Array.Empty<string>();
}
