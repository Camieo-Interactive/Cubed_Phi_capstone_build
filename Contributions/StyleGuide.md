# Cubed_Phi Unity Code Style Guide

This style guide outlines key conventions for writing clean, maintainable Unity code. It embraces functional programming paradigms, assumes all references are set in the Inspector, and minimizes explicit null-checks using null propagation. The member ordering should be as follows: **Public**, then **Protected**, then **Private**, with clear visual separators.

---

## File & Namespace Organization

- **Using Statements:**  
  Place all necessary `using` directives at the top and group related namespaces together.

  ```csharp
  using UnityEngine;
  using UnityEngine.Audio;
  using System.Collections;
  using System.Collections.Generic;
  ```

- **Static Imports:**  
  Use `using static` when importing constant classes or similar utilities.

  ```csharp
  using static Constants;
  ```

---

## Documentation & Comments

- **XML Comments:**  
  Use XML documentation (`///`) for classes and methods. Include `<summary>` and `<remarks>` sections as needed.

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

- **Inline Comments:**  
  Use inline comments to clarify complex logic while keeping the code self-explanatory.

  ```csharp
  // Compute the player's new position based on input.
  Vector2 newPosition = currentPosition + velocity * Time.deltaTime;
  ```

---

## Class & Field Structure

- **Member Ordering:**  
  Organize class members into three clear sections with visual separators. Order them as follows:
  1. **Public Members**
  2. **Protected Members**
  3. **Private Members**

  Use visual bars (e.g., `//  ------------------ Public ------------------`) to separate each section.

  ```csharp
  //  ------------------ Public ------------------
  [Header("Audio Settings")]
  [Tooltip("Audio clip for this sound.")]
  public AudioClip clip;

  //  ------------------ Protected ------------------
  // (Protected members go here)

  //  ------------------ Private ------------------
  private Rigidbody2D _rb;
  private float _moveSpeed;
  ```

- **Lifecycle Members:**  
  Lifecycle methods (e.g., `Awake()`, `Start()`, `FixedUpdate()`) should be marked **private** when possible.

  ```csharp
  //  ------------------ Private ------------------
  private void Awake() {
      _rb = GetComponent<Rigidbody2D>();
  }

  private void Start() {
      _moveSpeed = 5f;
  }

  private void FixedUpdate() {
      _rb.MovePosition(_rb.position + (Vector2)transform.up * _moveSpeed * Time.fixedDeltaTime);
  }
  ```

- **Public Fields:**  
  Use `[Tooltip("...")]` attributes to provide hints. Assume all references are set in the Inspector.

  ```csharp
  //  ------------------ Public ------------------
  [Header("Sound Details")]
  [Tooltip("Display name for this sound.")]
  public string soundName;
  ```

- **Private Fields:**  
  Group private fields in the private section and prefix them with an underscore.

  ```csharp
  //  ------------------ Private ------------------
  private float _dashSpeed;
  ```

---

## Method Organization

- **Lifecycle Methods:**  
  Use Unity lifecycle methods appropriately (e.g., `Awake()`, `Start()`, `FixedUpdate()`) and mark them private when possible.

  ```csharp
  private void Awake() {
      _rb = GetComponent<Rigidbody2D>();
  }

  private void Start() {
      _moveSpeed = 5f;
  }

  private void FixedUpdate() {
      _rb.MovePosition(_rb.position + (Vector2)transform.up * _moveSpeed * Time.fixedDeltaTime);
  }
  ```

- **Method Documentation:**  
  Document every public method with XML comments, clearly describing parameters and expected behaviors.

  ```csharp
  /// <summary>
  /// Moves the player in the specified direction.
  /// </summary>
  /// <param name="direction">Normalized vector representing movement direction.</param>
  public void Move(Vector2 direction) {
      Vector2 newPosition = direction is Vector2 d ? d * _moveSpeed * Time.deltaTime : Vector2.zero;
      _rb.MovePosition(_rb.position + newPosition);
  }
  ```

- **Helper Methods:**  
  Keep helper methods focused and single-purposed. Use clear, descriptive names.

  ```csharp
  //  ------------------ Private ------------------
  // Toggle the dash state for the player.
  private void ToggleDash() {
      isDashing = !isDashing;
      onDashToggle?.Invoke();
  }
  ```

---

## Naming Conventions

- **Classes & Methods:**  
  Use PascalCase.

  ```csharp
  public class PlayerMovement { ... }
  public void Dash() { ... }
  ```

- **Variables:**  
  - Public variables: Use PascalCase.  
  - Private variables: Use camelCase with an underscore prefix.

  ```csharp
  // Public variable
  public int Health;

  // Private variable
  private float _dashSpeed;
  ```

---

## Additional Best Practices

- **Inspector-Set References:**  
  Since all components and dependencies are assumed to be set in the Inspector, avoid repetitive null-checks. Use safe navigation (null propagation) when needed.

  ```csharp
  // Preferred approach:
  playerFX?.initialize();
  ```

- **Error Handling:**  
  Use `Debug.LogError` only for unexpected or critical issues.

  ```csharp
  // Log error only for unexpected scenarios.
  if (_rb == null) {
      Debug.LogError("Rigidbody2D is missing. Check the component assignment in the Inspector.");
  }
  ```

- **Functional Programming Paradigms:**  
  Favor immutability and concise expressions. Use lambda expressions and null propagation to streamline code.

  ```csharp
  // Using lambda for concise event invocation.
  public event System.Action onDashToggle = () => { };

  // Invoking the event using null propagation.
  onDashToggle?.Invoke();
  ```

- **Consistency:**  
  Follow the DRY (Don't Repeat Yourself) principle. Maintain consistent spacing, indentation, and naming conventions throughout your code.

  ```csharp
  // Consistent, clear, and concise method calls.
  private void Start() {
      InitializePlayer();
      InitializeHUD();
  }
  ```

- **Unity Specifics:**  
  Leverage Unity's lifecycle methods and features (like coroutines) appropriately.

  ```csharp
  /// <summary>
  /// Starts the cooldown coroutine for the player's dash ability.
  /// </summary>
  private IEnumerator DashCooldown(float duration) {
      yield return new WaitForSeconds(duration);
      onCooldownComplete?.Invoke();
  }
  ```

---

## LLVM Prompt for Code Formatting

Below is a prompt that contributors can copy and paste into an LLVM-based code formatter or LLM to ensure consistent adherence to the Cubed_Phi Unity Code Style Guide:

```plaintext
You are a code formatter and reviewer for our Unity projects. Please adhere to the following Cubed_Phi Unity Code Style Guide when analyzing, reformatting, or providing feedback on Unity code.

------------------
Cubed_Phi Unity Code Style Guide
------------------

This style guide outlines key conventions for writing clean, maintainable Unity code. It embraces functional programming paradigms, assumes all references are set in the Inspector, and minimizes explicit null-checks using null propagation. Member ordering should be as follows: Public, then Protected, then Private, with clear visual separators.

1. File & Namespace Organization
   - Using Statements: Place all necessary `using` directives at the top and group related namespaces together.
     Example:
     ```
     using UnityEngine;
     using UnityEngine.Audio;
     using System.Collections;
     using System.Collections.Generic;
     ```
   - Static Imports: Use `using static` for importing constant classes or similar utilities.
     Example:
     ```
     using static Constants;
     ```

2. Documentation & Comments
   - XML Comments: Use XML documentation (`///`) for classes and methods. Include `<summary>` and `<remarks>` sections as needed.
     Example:
     ```
     /// <summary>
     /// Manages the player's movement.
     /// </summary>
     /// <remarks>
     /// Adapted from Cubed Arcade's original implementation.
     /// See: https://github.com/XOR-SABER/Cubed-Arcade
     /// </remarks>
     public class PlayerMovement : MonoBehaviour { ... }
     ```
   - Inline Comments: Use inline comments to clarify complex logic while keeping the code self-explanatory.
     Example:
     ```
     // Compute the player's new position based on input.
     Vector2 newPosition = currentPosition + velocity * Time.deltaTime;
     ```

3. Class & Field Structure
   - Member Ordering: Organize class members into three sections with visual separators:
     - Public Members
     - Protected Members
     - Private Members
     
     Use visual bars, e.g.:
     ```
     //  ------------------ Public ------------------
     //  ------------------ Protected ------------------
     //  ------------------ Private ------------------
     ```
   - Lifecycle Members: Lifecycle methods (e.g., Awake(), Start(), FixedUpdate()) should be marked private when possible.
     Example:
     ```
     //  ------------------ Private ------------------
     private void Awake() {
         _rb = GetComponent<Rigidbody2D>();
     }
     private void Start() { ... }
     private void FixedUpdate() { ... }
     ```
   - Public Fields: Use [Tooltip("...")] attributes and assume all references are set in the Inspector.
     Example:
     ```
     //  ------------------ Public ------------------
     [Header("Sound Details")]
     [Tooltip("Display name for this sound.")]
     public string soundName;
     ```
   - Private Fields: Group private fields with an underscore prefix.
     Example:
     ```
     //  ------------------ Private ------------------
     private float _dashSpeed;
     ```

4. Method Organization
   - Lifecycle Methods: Use Unity lifecycle methods appropriately and mark them private when possible.
   - Method Documentation: Document every public method with XML comments, clearly describing parameters and expected behaviors.
     Example:
     ```
     /// <summary>
     /// Moves the player in the specified direction.
     /// </summary>
     /// <param name="direction">Normalized vector representing movement direction.</param>
     public void Move(Vector2 direction) { ... }
     ```
   - Helper Methods: Keep helper methods focused and single-purposed with clear, descriptive names.
     Example:
     ```
     //  ------------------ Private ------------------
     private void ToggleDash() { ... }
     ```

5. Naming Conventions
   - Classes & Methods: Use PascalCase.
     Example:
     ```
     public class PlayerMovement { ... }
     public void Dash() { ... }
     ```
   - Variables:
     - Public variables: PascalCase.
     - Private variables: camelCase with an underscore prefix.
     Example:
     ```
     public int Health;
     private float _dashSpeed;
     ```

6. Additional Best Practices
   - Inspector-Set References: Assume all components and dependencies are set in the Inspector, avoiding repetitive null-checks. Use safe navigation (null propagation) when needed.
     Example:
     ```
     playerFX?.initialize();
     ```
   - Error Handling: Use Debug.LogError only for unexpected or critical issues.
     Example:
     ```
     if (_rb == null) {
         Debug.LogError("Rigidbody2D is missing. Check the component assignment in the Inspector.");
     }
     ```
   - Functional Programming Paradigms: Favor immutability and concise expressions. Use lambda expressions and null propagation.
     Example:
     ```
     public event System.Action onDashToggle = () => { };
     onDashToggle?.Invoke();
     ```
   - Consistency: Follow the DRY (Don't Repeat Yourself) principle. Maintain consistent spacing, indentation, and naming conventions.
     Example:
     ```
     private void Start() {
         InitializePlayer();
         InitializeHUD();
     }
     ```
   - Unity Specifics: Leverage Unity's lifecycle methods and features (e.g., coroutines) appropriately.
     Example:
     ```
     private IEnumerator DashCooldown(float duration) {
         yield return new WaitForSeconds(duration);
         onCooldownComplete?.Invoke();
     }
     ```

Please format all Unity code according to these guidelines.
```