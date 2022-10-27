/* Created by: Justin Butler
 * Purpose: To showcase the basic ways that you can move a single object.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Movement : MonoBehaviour
{
    ///////////////////////////////////
    // Fields
    ///////////////////////////////////

    /// <summary>
    /// The amount of time for the movement to happen in.
    /// </summary>
    public float moveDuration = 5.0f;
    /// <summary>
    /// A float can add a great amount of control using math. In this case, we are adding a speed modifier to control the speed of our object.
    /// </summary>
    public float speed = 3.0f;
    /// <summary>
    /// Used with SmoothDamp to smooth our approach to the target.
    /// </summary>
    public float smoothTime = 0.5f;
    /// <summary>
    /// You can have a Vector3 as a point of reference for movement as well.
    /// </summary>
    public Vector3 target = new Vector3(0, 0, 2);
    /// <summary>
    /// We need a reference to our object's current velocity.
    /// </summary>
    Vector3 currentVelocity;
    /// <summary>
    /// Allows us to talk to this object's Rigidbody (must be using Rigidbody for this to work).
    /// </summary>
    public Rigidbody rb;
    /// <summary>
    /// The force we are going to exert on our object.
    /// </summary>
    public float forceStrength = 20;
    /// <summary>
    /// A float that controls the amount of force you use in physic based objects.
    /// </summary>
    float forceControl;

    ///////////////////////////////////
    // Order of Execution
    ///////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
            ImmediateMovement();

        if (Input.GetKey(KeyCode.Alpha2))
            ShiftMovement();

        if (Input.GetKey(KeyCode.Alpha3))
            TranslateMovement_v1();

        if (Input.GetKey(KeyCode.Alpha4))
            TranslateMovement_v2();

        if (Input.GetKey(KeyCode.Alpha5))
            MoveTowardsMovement();

        if (Input.GetKey(KeyCode.Alpha6))
            SmoothDampMovement();

        if (Input.GetKey(KeyCode.KeypadEnter))
            StartCoroutine(LerpMovement(target));

        if (Input.GetKey(KeyCode.Space))
            forceControl = Input.GetAxis("Vertical");
    }

    // FixedUpdate is called every fixed frame-rate frame
    private void FixedUpdate()
    {
        // FixedUpdate is where you should put physic based functions so that things are correctly in sync. Additional note, try to keep player inputs inside Update and reference it over to FixedUpdate.
        //if (Input.GetKey(KeyCode.Space))
            rb.AddForce(Vector3.up * forceControl * forceStrength);
    }

    ///////////////////////////////////
    // Public Methods
    ///////////////////////////////////

    /// <summary>
    /// This method will move the object to a spot immediately.
    /// </summary>
    public void ImmediateMovement()
    {
        // In this case, this method will always move the position to (1,2,3) in the world space.
        transform.position = new Vector3(1, 2, 3);
    }

    /// <summary>
    /// This method will shift the object relative to its current position.
    /// </summary>
    public void ShiftMovement()
    {
        // In this case, this method will shift the object up 1 in the y direction.
        transform.position += new Vector3(0, 1, 0);
    }

    /// <summary>
    /// This method uses Unity's built in Translate method.
    /// </summary>
    public void TranslateMovement_v1()
    {
        // Translate is similar to ShiftMovement where moves a set amount each time it's called.
        // However, you can do a lot more with Translate because of the overloads it has.
        // In this case, we are using the first overload which utilize Vector3 translation.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    /// <summary>
    /// This method expands upon the TranslateMovement_v1 while also using controls.
    /// </summary>
    public void TranslateMovement_v2()
    {
        // There are multiple ways to control an object, and I will cover that in a seprate script, for now we will use the basic Unity controls.
        // It should be noted that the axis are just inputting a value. If we wanted to move up, we could swap "Vertical" to the y vector.
        Vector3 movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // It should be noted that we had to normalize the value from above ^ so that diagonals are not faster then single direction.
        // The other method of doing this is by clamping the magnitude.
        #region Other method using ClampMagnitude
        //movementDirection = Vector3.ClampMagnitude(movementDirection, 1);
        #endregion
        transform.Translate(movementDirection.normalized * speed * Time.deltaTime);
    }

    /// <summary>
    /// Moves the object towards a target.
    /// </summary>
    public void MoveTowardsMovement()
    {
        // This is the basic of moving an object towards another target.
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    /// <summary>
    /// Moves the object towards a target, but with added control of speed.
    /// </summary>
    public void SmoothDampMovement()
    {
        // Not going to lie, still lost on how we get our current velocity using this v. If it does work, the object should adjust its speed depending on the position of the target. Good for camera work.
        transform.position = Vector3.SmoothDamp(transform.position, target, ref currentVelocity, smoothTime);
    }

    /// <summary>
    /// Moves the object in a fixed manner based on data.
    /// </summary>
    IEnumerator LerpMovement(Vector3 targetPosition)
    {
        // First, you need the current position of your object.
        Vector3 startPosition = transform.position;
        // Second, you need a value to keep track of time.
        float timeElapsed = 0.0f;

        // A while loop will check after each completion to see if the conditions are met. If it isn't, it will keep running.
        while (timeElapsed < moveDuration)
        {
            // The t value (timeElapsed/moveDuration) dictates where the position is on the interpolation. If startPosition is a and targetPosition is b, t is the point between these two. a = 0 b = 1 0?t<1
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / moveDuration);
            // This will increase timeElapsed so that the next time we do this loop, the position will be different.
            timeElapsed += Time.deltaTime;
            // This is the point where execution pauses and resumes in the following frame.
            yield return null;
        }

        // When the while loop conditions is false, it sets the final position to the targetPosition.
        transform.position = targetPosition;
    }


}
