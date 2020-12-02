using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_2
{
    public class Risk
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SourceRisk { get; set; }
        public string ObjectImpact { get; set; }
        public bool PrivacyViolatation { get; set; }
        public bool IntegrityViolatation { get; set; }
        public bool AvaliabilityViolatation { get; set; }

        
        public Risk()
        {
            Id = 0;
            Name = ""; 
            Description = "";
            SourceRisk = "";
            ObjectImpact = "";
            PrivacyViolatation = false;
            IntegrityViolatation = false;
            AvaliabilityViolatation = false;
        }

        public Dictionary<string, string> ToDict()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Идентификатор угрозы:", GetFullNameId());
            dict.Add("Наименование угрозы:", Name);
            dict.Add("Описание угрозы:", Description);
            dict.Add("Источник угрозы", SourceRisk);
            dict.Add("Объект воздействия угрозы:", ObjectImpact);
            dict.Add("Нарушение конфиденциальности:", GetStatusAnswer(PrivacyViolatation));
            dict.Add("Нарушение целостности:", GetStatusAnswer(IntegrityViolatation));
            dict.Add("Нарушение доступности:", GetStatusAnswer(AvaliabilityViolatation));
            return dict;
        }

        public struct RiskStruct
        {
            public string Name { get; set; }
            public string LastVal { get; set; }
            public string NewVal { get; set; }

            public RiskStruct(string name, string lastVal, string newVal)
            {
                Name = name;
                LastVal = lastVal;
                NewVal = newVal;
            }
        }

        public Risk GetCompareRisk(Risk risk)
        {
            Risk r = new Risk();

            this.Description = this.Description == risk.Description ? this.Description : risk.Description;
            this.SourceRisk = this.SourceRisk == risk.SourceRisk ? this.SourceRisk : risk.SourceRisk;
            this.ObjectImpact = this.ObjectImpact == risk.ObjectImpact ? this.ObjectImpact : risk.ObjectImpact;
            this.PrivacyViolatation = this.PrivacyViolatation == risk.PrivacyViolatation ? this.PrivacyViolatation : risk.PrivacyViolatation;
            this.IntegrityViolatation = this.IntegrityViolatation == risk.IntegrityViolatation ? this.IntegrityViolatation : risk.IntegrityViolatation;
            this.AvaliabilityViolatation = this.AvaliabilityViolatation == risk.AvaliabilityViolatation ? this.AvaliabilityViolatation : risk.AvaliabilityViolatation;
            
            return r;
        }

        public static string GetStatusAnswer(bool answer)
        {
            return answer ? "Да" : "Нет";
        }

        
        public string GetFullNameId()
        {
            return "УБИ." + Id.ToString();
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Risk risk = (Risk)obj;
                return (this.Id == risk.Id) && 
                    (this.Name == risk.Name) && 
                    (this.Description == risk.Description) && 
                    (this.SourceRisk == risk.SourceRisk) && 
                    (this.ObjectImpact == risk.ObjectImpact) && 
                    (this.PrivacyViolatation == risk.PrivacyViolatation) && 
                    (this.IntegrityViolatation == risk.IntegrityViolatation) && 
                    (this.AvaliabilityViolatation == risk.AvaliabilityViolatation);
            }
        }
    }
}
