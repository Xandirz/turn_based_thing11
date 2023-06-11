using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class CooldownService
    {
        public string usedAbility;
        
        private GameObject button;
        public int cooldown = 0;
        private string buttonName;
        private string attackName;

        public CooldownService(GameObject button, string buttonName, string attackName, string usedAbility)
        {
            this.usedAbility = usedAbility;
            this.button = button;
            this.buttonName = buttonName;
            this.attackName = attackName;
        }

        public void Handle()
        {
            if (cooldown > 0)
            {
                cooldown--;
                UpdateCooldownText();
                if (cooldown == 0)
                {
                    button.GetComponent<TextMeshPro>().text = buttonName;
                }
            }
        }

        public void UpdateCooldownText()
        {
            button.GetComponent<TextMeshPro>().text = attackName + cooldown + " ходов";
        }
    }
}