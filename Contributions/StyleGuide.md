---

### **Cubed_Phi Unity Code Style Guide**

This style guide is designed to ensure clean, maintainable Unity code. It embraces a structured and organized approach, keeping in mind principles like readability, modularity, and the use of Unity-specific features.

---

#### 1. **File & Namespace Organization**
   - **Using Statements**: Place all necessary `using` directives at the top, grouping related namespaces together.
     - Example:
     ```csharp
     using UnityEngine;
     using UnityEngine.Audio;
     using System.Collections;
     using System.Collections.Generic;
     ```
   - **Static Imports**: Use `using static` for importing constant classes or similar utilities.
     - Example:
     ```csharp
     using static Constants;
     ```

---

#### 2. **Documentation & Comments**
   - **XML Comments**: Use XML documentation (`///`) for classes, methods, and properties. Include `<summary>` and `<remarks>` sections as needed to describe behaviors and provide context.
     - Example:
     ```csharp
     /// <summary>
     /// Manages the player's movement.
     /// </summary>
     /// <remarks>
     /// Adapted from Cubed Arcade's original implementation.
     /// See: https://github.com/XOR-SABER/Cubed-Arcade
     /// </remarks>
     public class PlayerMovement : MonoBehaviour { ... }
     ```
   - **Inline Comments**: Use inline comments only when necessary to clarify complex logic.
     - Example:
     ```csharp
     // Compute the player's new position based on input.
     Vector2 newPosition = currentPosition + velocity * Time.deltaTime;
     ```

---

#### 3. **Class & Field Structure**

   - **Member Ordering**: Organize class members into three sections with clear visual separators:
     1. **Public Members**: Public variables and methods.
     2. **Protected Members**: Protected variables and methods.
     3. **Private Members**: Private variables and methods.

     Visual separators (i.e., `//  ------------------ Public ------------------`) should be used for clarity.

     - Example:
     ```csharp
     //  ------------------ Public ------------------
     public int Health;
     public void Move(Vector2 direction) { ... }
     
     //  ------------------ Protected ------------------
     protected float speed;
     
     //  ------------------ Private ------------------
     private Rigidbody2D _rb;
     private void Start() { ... }
     ```

   - **Lifecycle Members**: Unity lifecycle methods (e.g., `Awake()`, `Start()`, `FixedUpdate()`) should be marked **private** when possible to limit their visibility.
     - Example:
     ```csharp
     //  ------------------ Private ------------------
     private void Awake() {
         _rb = GetComponent<Rigidbody2D>();
     }
     ```

---

#### 4. **Field Organization**

   - **Public Fields**: Should be declared at the top of the class. Use `[Tooltip("...")]` attributes for clarity, especially for references set in the Inspector. They should be in **PascalCase**.
     - Example:
     ```csharp
     //  ------------------ Public ------------------
     [Header("Sound Settings")]
     [Tooltip("Volume of the sound.")]
     public float volume;
     ```

   - **Protected Fields**: Declare them after public fields. Use **camelCase** with an underscore (`_`) as a prefix to indicate they are protected variables.
     - Example:
     ```csharp
     //  ------------------ Protected ------------------
     protected float _speed;
     ```

   - **Private Fields**: Declare them after protected fields. Use **camelCase** with an underscore (`_`) as a prefix for clarity and consistency. These fields should ideally be kept private unless specific functionality requires them to be accessible.
     - Example:
     ```csharp
     //  ------------------ Private ------------------
     private int _health;
     private float _dashSpeed;
     ```

   - **Arrays and Lists**: Use descriptive names and avoid abbreviations.
     - Example:
     ```csharp
     //  ------------------ Private ------------------
     private List<GameObject> enemies;
     ```

---

#### 5. **Method Organization**

   - **Public Methods**: Declare public methods for operations that need to be accessed outside the class. Document these methods using XML comments to describe the functionality and parameters clearly.
     - Example:
     ```csharp
     /// <summary>
     /// Moves the player in the specified direction.
     /// </summary>
     /// <param name="direction">Normalized vector representing movement direction.</param>
     public void Move(Vector2 direction) { ... }
     ```

   - **Protected Methods**: Declare protected methods after public methods. These are for functionality that is intended to be used in subclasses.
     - Example:
     ```csharp
     //  ------------------ Protected ------------------
     protected void ToggleVisibility() { ... }
     ```

   - **Private Methods**: Declare private methods at the bottom of the class or after protected methods. Keep them focused and concise, adhering to the **single responsibility** principle.
     - Example:
     ```csharp
     //  ------------------ Private ------------------
     private void InitializeHealth() { ... }
     ```

---

#### 6. **Naming Conventions**
   - **Classes & Methods**: Use **PascalCase** for class and method names.
     - Example:
     ```csharp
     public class PlayerMovement { ... }
     public void Dash() { ... }
     ```

   - **Variables**:
     - **Public Variables**: Use **PascalCase** for public variables.
       - Example:
       ```csharp
       public float DashSpeed;
       ```
     - **Private and Protected Variables**: Use **camelCase** with an underscore (`_`) prefix for private and protected variables.
       - Example:
       ```csharp
       private float _health;
       protected float _speed;
       ```

   - **Events**: Use **PascalCase** for event names.
     - Example:
     ```csharp
     public event System.Action OnDashComplete;
     ```

---

#### 7. **Additional Best Practices**

   - **Inspector-Set References**: Assume all references (e.g., `healthComponent`, `animator`) are set in the Inspector. Avoid null-checks unless absolutely necessary.
     - Example:
     ```csharp
     playerFX?.initialize();
     ```

   - **Error Handling**: Use `Debug.LogError` for unexpected or critical issues that require immediate attention. Avoid excessive logging in production code.
     - Example:
     ```csharp
     if (_rb == null) {
         Debug.LogError("Rigidbody2D is missing. Check the component assignment in the Inspector.");
     }
     ```

   - **Functional Programming Paradigms**: Favor immutability and concise expressions. Use lambda expressions where appropriate and use **null propagation** (`?.`).
     - Example:
     ```csharp
     public event System.Action onDashToggle = () => { };
     onDashToggle?.Invoke();
     ```

   - **Consistency**: Follow the **DRY (Don't Repeat Yourself)** principle. Maintain consistent spacing, indentation, and naming conventions across the codebase.

   - **Unity-Specific Practices**: Leverage Unity's lifecycle methods and coroutines appropriately to manage performance and timing.
     - Example:
     ```csharp
     private IEnumerator DashCooldown(float duration) {
         yield return new WaitForSeconds(duration);
         onCooldownComplete?.Invoke();
     }
     ```

---

## LLVM Prompt for Code Formatting

Below is a prompt that contributors can copy and paste into an LLVM-based code formatter or LLM to ensure consistent adherence to the Cubed_Phi Unity Code Style Guide:

```paintext
You are a code formatter and reviewer for our Unity projects. Please adhere to the following Cubed_Phi Unity Code Style Guide when analyzing, reformatting, or providing feedback on Unity code.

------------------
Cubed_Phi Unity Code Style Guide
------------------

This style guide outlines key conventions for writing clean, maintainable Unity code. It embraces functional programming paradigms, assumes all references are set in the Inspector, and minimizes explicit null-checks using null propagation. Member ordering should be as follows: Public, then Protected, then Private, with clear visual separators.

1. **File & Namespace Organization**
   - **Using Statements**: Place all necessary `using` directives at the top and group related namespaces together.
     Example:
     ```csharp
     using UnityEngine;
     using UnityEngine.Audio;
     using System.Collections;
     using System.Collections.Generic;
     ```
   - **Static Imports**: Use `using static` for importing constant classes or similar utilities.
     Example:
     ```csharp
     using static Constants;
     ```

2. **Documentation & Comments**
   - **XML Comments**: Use XML documentation (`///`) for classes and methods. Include `<summary>` and `<remarks>` sections as needed.
     Example:
     ```csharp
     /// <summary>
     /// Manages the player's movement.
     /// </summary>
     /// <remarks>
     /// Adapted from Cubed Arcade's original implementation.
     /// See: https://github.com/XOR-SABER/Cubed-Arcade
     /// </remarks>
     public class PlayerMovement : MonoBehaviour { ... }
     ```
   - **Inline Comments**: Use inline comments to clarify complex logic while keeping the code self-explanatory.
     Example:
     ```csharp
     // Compute the player's new position based on input.
     Vector2 newPosition = currentPosition + velocity * Time.deltaTime;
     ```

3. **Class & Field Structure**
   - **Member Ordering**: Organize class members into three sections with visual separators:
     - Public Members
     - Protected Members
     - Private Members
     
     Use visual bars to separate these sections:
     ```csharp
     //  ------------------ Public ------------------
     //  ------------------ Protected ------------------
     //  ------------------ Private ------------------
     ```

   - **Lifecycle Members**: Lifecycle methods (e.g., Awake(), Start(), FixedUpdate()) should be marked private when possible.
     Example:
     ```csharp
     //  ------------------ Private ------------------
     private void Awake() {
         _rb = GetComponent<Rigidbody2D>();
     }
     private void Start() { ... }
     private void FixedUpdate() { ... }
     ```

   - **Public Fields**: Public fields should be placed at the top of the class. Use `[Tooltip("...")]` attributes and assume all references are set in the Inspector.
     Example:
     ```csharp
     //  ------------------ Public ------------------
     [Header("Sound Details")]
     [Tooltip("Display name for this sound.")]
     public string soundName;
     ```

   - **Protected Fields**: Declare protected fields after public fields. Use camelCase with an underscore prefix to indicate they are protected.
     Example:
     ```csharp
     //  ------------------ Protected ------------------
     protected float _speed;
     ```

   - **Private Fields**: Declare private fields after protected fields. These should use camelCase with an underscore prefix.
     Example:
     ```csharp
     //  ------------------ Private ------------------
     private float _dashSpeed;
     ```

4. **Method Organization**
   - **Lifecycle Methods**: Use Unity lifecycle methods appropriately and mark them private when possible.
   - **Method Documentation**: Document every public method with XML comments, clearly describing parameters and expected behaviors.
     Example:
     ```csharp
     /// <summary>
     /// Moves the player in the specified direction.
     /// </summary>
     /// <param name="direction">Normalized vector representing movement direction.</param>
     public void Move(Vector2 direction) { ... }
     ```

   - **Helper Methods**: Keep helper methods focused and single-purposed with clear, descriptive names.
     Example:
     ```csharp
     //  ------------------ Private ------------------
     private void ToggleDash() { ... }
     ```

5. **Naming Conventions**
   - **Classes & Methods**: Use **PascalCase** for class and method names.
     Example:
     ```csharp
     public class PlayerMovement { ... }
     public void Dash() { ... }
     ```

   - **Variables**:
     - **Public variables**: Use **PascalCase** for public variables.
     Example:
     ```csharp
     public int Health;
     ```

     - **Private and Protected variables**: Use **camelCase** with an underscore prefix (`_`) for private and protected variables.
     Example:
     ```csharp
     private float _dashSpeed;
     protected float _speed;
     ```

6. **Additional Best Practices**
   - **Inspector-Set References**: Assume all components and dependencies are set in the Inspector, avoiding repetitive null-checks. Use safe navigation (null propagation) when needed.
     Example:
     ```csharp
     playerFX?.initialize();
     ```

   - **Error Handling**: Use `Debug.LogError` only for unexpected or critical issues.
     Example:
     ```csharp
     if (_rb == null) {
         Debug.LogError("Rigidbody2D is missing. Check the component assignment in the Inspector.");
     }
     ```

   - **Functional Programming Paradigms**: Favor immutability and concise expressions. Use lambda expressions and null propagation.
     Example:
     ```csharp
     public event System.Action onDashToggle = () => { };
     onDashToggle?.Invoke();
     ```

   - **Consistency**: Follow the **DRY (Don't Repeat Yourself)** principle. Maintain consistent spacing, indentation, and naming conventions.
     Example:
     ```csharp
     private void Start() {
         InitializePlayer();
         InitializeHUD();
     }
     ```

   - **Unity-Specific Practices**: Leverage Unity's lifecycle methods and features (e.g., coroutines) appropriately to manage performance and timing.
     Example:
     ```csharp
     private IEnumerator DashCooldown(float duration) {
         yield return new WaitForSeconds(duration);
         onCooldownComplete?.Invoke();
     }
     ```

Please format all Unity code according to these guidelines.
```