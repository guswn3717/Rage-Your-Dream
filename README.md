# 🥊 Rage Your Dream

_A competitive 1v1 multiplayer boxing game focused on timing, rhythm, and psychological reads._

---

## 🎯 Game Overview

**Game Type**: 1v1 Multiplayer Boxing  
**Victory Condition**: Knock your opponent down 3 times to win the match.

---

## 💡 Core Systems

### 1. HP & HL Bars
- **HP (Health Points)**: Standard health.
- **HL (Heal Limit)**: Maximum recoverable HP.  
  - Light attacks or guarded hits reduce HP only.  
  - Strong attacks reduce both HP and HL.  
  - HL regenerates slowly unless hit by strong attacks.

### 2. Blocking (Guard)
- Guard has limited durability.
- Balanced so that spamming light attacks will just barely not break a full guard.
- Strong attacks deal much higher guard damage.

### 3. Dodging
- **Normal Dodge**: Squeaky boxing shoe sound.
- **Perfect Dodge**:  
  - Slow-motion trigger  
  - Guarantees an unavoidable follow-up hit  
- Both dodge and strong attacks have after-delay (cooldown).

### 4. First Hit System
- Only the **first light punch** in a combo has startup delay.
- Follow-up punches are rapid.
- Prevents mash-to-win behavior after combos.

### 5. Bars & UI Visibility
- **Visible to both players**: HP, HL, SP (Stamina), BL (Block)
- **Visible to self only**: SM (Special Meter)

---

## 💥 Strong Attacks
- Delayed wind-up before activation.
- Deal heavy damage to both HP and HL.
- If blocked: no HL damage, but still hurts guard and stamina.

---

## ⚠️ Ultimate Moves (Finishers)
- High-risk, high-reward system.
- Missing a finisher results in **Stamina Broken** (SB) state, leaving the player vulnerable.

---

## 🧠 Design Philosophy

- Emphasizes **mind games**, **rhythm**, and **timing** over raw reflex.
- Not a button masher.
- Every action must be **deliberate**, requiring reads and counter-reads.

---

## 🔊 Sound Design

| Action            | Sound Description              |
|-------------------|--------------------------------|
| Light Attack       | Soft sandbag hit               |
| Strong Attack      | Loud sandbag hit               |
| Perfect Dodge      | *Arthur’s Dead Eye* (RDR2) SFX |
| Normal Dodge       | Squeaky boxing shoes           |
| Feint              | Whizzing / Lock-on pitching    |

---

## 🧩 Code Structure

- Modular Unity C# structure.
- Each mechanic (HP, HL, guard, etc.) is implemented as an isolated `.cs` file.
- Clean separation of combat logic, UI, and player control.

---

## 🎮 Match Flow

- A round ends when one player is knocked down.
- **3 knockdowns = match loss.**
- Stamina is crucial to manage attacking, defending, and dodging effectively.

---

## 🛠️ Technologies

- **Engine**: Unity  
- **Language**: C#  
- **Multiplayer**: Photon / Unity Netcode (TBD)  

---

## 📌 Status

Development in progress – early prototyping & combat system refinement phase.

---

## 🤝 Contribution

Contributors welcome. Submit ideas, balance suggestions, or pull requests!

---

## 📄 License

TBD (To be decided)

---

