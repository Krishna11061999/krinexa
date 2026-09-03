namespace Krinexa.Domain.Enums;

public enum UserType { Talent, Student, Intern, Client, Interviewer, Admin }
public enum ProfileType { Student, Intern, Junior, Senior }
public enum Proficiency { Beginner, Intermediate, Advanced, Expert }
public enum MatchStatus { Pending, Shortlisted, Rejected, Accepted, Withdrawn }
public enum InterestStatus { Pending, Shortlisted, Declined }
public enum InterviewStatus { Requested, Confirmed, Rescheduled, Completed, Rejected, Cancelled }
public enum PaymentStatus { Pending, Verified, Rejected }
public enum RequirementStatus { Open, OpenForInterest, Matching, Filled, Closed }
public enum SenderRole { Client, KrinexaTeam, Talent }
