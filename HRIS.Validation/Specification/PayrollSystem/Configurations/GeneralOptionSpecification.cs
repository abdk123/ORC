using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Configurations
{
    public class GeneralOptionSpecification : Validates<GeneralOption>
    {
        public GeneralOptionSpecification()
        {
            IsDefaultForType();
            Check(x => x.TaxThreshold, y => typeof(GeneralOption).GetProperty("TaxThreshold").GetTitle()).Required().GreaterThan(0);
            Check(x => x.TotalMonthDays, y => typeof(GeneralOption).GetProperty("TotalMonthDays").GetTitle()).Required().GreaterThan(0).And.LessThanEqualTo(31);
            Check(x => x.TotalDayHours, y => typeof(GeneralOption).GetProperty("TotalDayHours").GetTitle()).Required().GreaterThan(0).And.LessThanEqualTo(24);

            Check(x => x.OvertimeBenefit, y => typeof(GeneralOption).GetProperty("OvertimeBenefit").GetTitle())
                .Required()
                .Expect((generalOption, overtimeBenefit) => overtimeBenefit.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.RecycledLeaveBenefit, y => typeof(GeneralOption).GetProperty("RecycledLeaveBenefit").GetTitle())
                .Required()
                .Expect((generalOption, recycledLeaveBenefit) => recycledLeaveBenefit.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.TaxDeduction, y => typeof(GeneralOption).GetProperty("TaxDeduction").GetTitle())
                .Required()
                .Expect((generalOption, taxDeduction) => taxDeduction.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));


            Check(x => x.LeaveDeduction, y => typeof(GeneralOption).GetProperty("LeaveDeduction").GetTitle())
                .Required()
                .Expect((generalOption, leaveDeduction) => leaveDeduction.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.PenaltyDeduction, y => typeof(GeneralOption).GetProperty("PenaltyDeduction").GetTitle())
                .Required()
                .Expect((generalOption, penaltyDeduction) => penaltyDeduction.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.AbsenceDaysDeduction, y => typeof(GeneralOption).GetProperty("AbsenceDaysDeduction").GetTitle())
                .Required()
                .Expect((generalOption, absenceDaysDeduction) => absenceDaysDeduction.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.NonAttendanceDeduction, y => typeof(GeneralOption).GetProperty("NonAttendanceDeduction").GetTitle())
                .Required()
                .Expect((generalOption, nonAttendanceDeduction) => nonAttendanceDeduction.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.LatenessDeduction, y => typeof(GeneralOption).GetProperty("LatenessDeduction").GetTitle())
                .Required()
                .Expect((generalOption, latenessDeduction) => latenessDeduction.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.RewardBenefit, y => typeof(GeneralOption).GetProperty("RewardBenefit").GetTitle())
                .Required()
                .Expect((generalOption, rewardBenefit) => rewardBenefit.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

        }
    }
}