// Person class for P1-GradeBook

public class Person {
    protected String firstName;
    protected String lastName;
    protected int age;
    
    // Default constructor
    public Person() {
        firstName = "";
        lastName = "";
        age = 0;
    }
    
    // Constructs a Person from data sent as parameters
    public Person(String first, String last, int age) {
        firstName = first;
        lastName = last;
        this.age = age;
    }

    // Person increases in age by one year
    public void birthday() {
      age++;
    }
    
    // Return String representation
    public String toString() {
        return firstName + " " + lastName + " " + age;
    }
    
    // Testing the Person class
    public static void main(String[] args) {
        Person prof = new Person("Amy", "Briggs", 39);
        System.out.println(prof);
        prof.birthday();
        System.out.println(prof);
    }
}
