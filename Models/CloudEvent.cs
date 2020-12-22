using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CloudEventManager.Models
{
	/// <summary>
	/// CloudEvent is a vendor-neutral specification for defining the format of event data.
	/// https://github.com/cloudevents/spec/blob/master/spec.md
	/// </summary>
	public class CloudEvent
	{
		public CloudEvent()
		{
		}

		/// <summary>
		/// The event payload. 
		/// </summary>
		/// <remarks>
		/// This specification does not place any restriction on the type of this information. It is encoded into a media format which is specified by the datacontenttype attribute (e.g. application/json), and adheres to the dataschema format when those repspective attributes are present.
		/// 
		/// If data's native syntax, or its syntax based on the datacontenttype attribute if present, can not be copied directly into the desired serialization format, and therefore needs to be further encoded, then the datacontentencoding attribute MUST include the encoding mechanism used.
		/// </remarks>
		[JsonProperty(PropertyName = "data")]
		public string Data { get; set; }

		/// <summary>
		/// Describes the content encoding for data.
		/// </summary>
		/// <remarks>
		/// There are cases where the value of data might need to be encoded so that it can be carried within the serialization format being used. For example, in JSON, binary data will likely need to be Base64 encoded. When this attribute is set, the consumer can use its value to know how to decode data value to retrieve its original contents.
		/// 
		/// If this attribute is supported, then the "Base64" encoding as defined in RFC 2045 Section 6.8 MUST be supported.
		/// </remarks>
		[JsonProperty(PropertyName = "datacontentencoding")]
		public string DataContentEncoding { get; set; }

		/// <summary>
		/// Content type of <see cref="Data"/> value.
		/// </summary>
		/// <remarks>
		/// This attribute enables data to carry any type of content, whereby format and encoding might differ from that of the chosen event format. For example, an event rendered using the JSON envelope format might carry an XML payload in data, and the consumer is informed by this attribute being set to "application/xml". The rules for how data content is rendered for different datacontenttype values are defined in the event format specifications; for example, the JSON event format defines the relationship in section 3.1.
		/// 
		/// When this attribute is omitted, data simply follows the event format's encoding rules. For the JSON event format, the data value can therefore be a JSON object, array, or value.
		/// 
		/// For the binary mode of some of the CloudEvents transport bindings, where the data content is immediately mapped into the payload of the transport frame, this field is directly mapped to the respective transport or application protocol's content-type metadata property. Normative rules for the binary mode and the content-type metadata mapping can be found in the respective transport mapping specifications.
		/// </remarks>
		[JsonProperty(PropertyName = "datacontenttype")]
		public string DataContentType { get; set; }

		/// <summary>
		/// Identifies the schema that <see cref="Data"/> adheres to. 
		/// </summary>
		/// <remarks>
		/// Incompatible changes to the schema SHOULD be reflected by a different URI. See Versioning of Attributes in the Primer for more information.
		/// </remarks>
		[JsonProperty(PropertyName = "dataschema")]
		public Uri DataSchema { get; set; }

		/// <summary>
		/// This attribute contains a value describing the type of event related to the originating occurrence. 
		/// </summary>
		/// <remarks>
		/// Often this attribute is used for routing, observability, policy enforcement, etc. The format of this is producer defined and might include information such as the version of the type.
		/// </remarks>
		[JsonProperty(PropertyName = "type")]
		public string EventType { get; set; }

		/// <summary>
		/// Identifies the event.
		/// </summary>
		/// <remarks> 
		/// Producers MUST ensure that source + id is unique for each distinct event. If a duplicate event is re-sent (e.g. due to a network error) it MAY have the same id. Consumers MAY assume that Events with identical source and id are duplicates.
		/// </remarks>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Identifies the context in which an event happened. 
		/// </summary>
		/// <remarks>
		/// Often this will include information such as the type of the event source, the organization publishing the event or the process that produced the event. The exact syntax and semantics behind the data encoded in the URI is defined by the event producer.
		/// </remarks>
		[JsonProperty(PropertyName = "source")]
		public string Source { get; set; }

		/// <summary>
		/// The version of the CloudEvents specification which the event uses.
		/// </summary>
		/// <remarks>
		/// This enables the interpretation of the context. Compliant event producers MUST use a value of 0.4-wip when referring to this version of the specification.
		/// </remarks>
		[JsonProperty(PropertyName = "specversion")]
		public string SpecVersion { get; set; } = "0.1";

		/// <summary>
		/// This describes the subject of the event in the context of the event producer (identified by source).
		/// </summary>
		/// <remarks>
		/// In publish-subscribe scenarios, a subscriber will typically subscribe to events emitted by a source, but the source identifier alone might not be sufficient as a qualifier for any specific event if the source context has internal sub-structure.
		/// 
		/// Identifying the subject of the event in context metadata (opposed to only in the data payload) is particularly helpful in generic subscription filtering scenarios where middleware is unable to interpret the data content.In the above example, the subscriber might only be interested in blobs with names ending with '.jpg' or '.jpeg' and the subject attribute allows for constructing a simple and efficient string-suffix filter for that subset of events.
		/// </remarks>
		[JsonProperty(PropertyName = "subject")]
		public string Subject { get; set; }

		/// <summary>
		/// Timestamp of when the occurrence happened.
		/// </summary>
		/// <remarks>
		/// If the time of the occurrence cannot be determined then this attribute MAY be set to some other time (such as the current time) by the CloudEvents producer, however all producers for the same source MUST be consistent in this respect. In other words, either they all use the actual time of the occurrence or they all use the same algorithm to determine the value used.
		/// </remarks>
		[JsonProperty(PropertyName = "time")]
		public DateTime Time { get; set; }
	}
}
