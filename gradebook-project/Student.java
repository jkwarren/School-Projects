import java.util.ArrayList;

// Student class for P1-GradeBook
// Name:
// Section (A or B):

public class Student extends Person {
    private String major;
    private String username;
    //private ArrayList<Person> 
    private ArrayList<Grade> grades;
    
    // Default constructor
    public Student() {
        major = null;
        username = null;
        grades = new ArrayList<Grade>();
    }
    
    public Student(String first, String last, int age, String major, String id) {
        super(first, last, age);  // Has to be first line in subclass constructor
        // if you don't call the super constructor, Java will implicitly call super()
        // so the superclass needs a default constructor
        this.major = major;
        username = id;
        grades = new ArrayList<Grade>();
    
    }
    
    // No need to qualify firstName or lastName as they are inherited from Person
    // i.e., since there isn't a firstName or lastName field in Student, Java goes
    // up the inheritance chain to find one
    public String getNameAndMajor() {
        return firstName + " " + lastName + " " + major;
    }
    public String getUsername(){
        return username;
    }
    public String getMajor(){
        return major;
    }
    public String getID(){
        return username;
    }
    
    // Have to qualify toString() call below, otherwise infinite recursion
    public String toString() {
        String result = "";
        for (Grade x: grades){
            result += x.toString();
        }
        
        return super.toString() + " " + username + " " + major + " " + result + '\n'; //adding grades so that they print
    }
    
    // adding grades
    public void addGrade(Grade newGrade){
        grades.add(newGrade);
    }

    // getting the print out of grades
    public String getGradesList(){
        String result = "";
        for (Grade x: grades){
            result += x.toString();
        }
        return result;
    }
    public ArrayList<Grade> getGrades(){
        
        return grades;
    }
    // Testing the Student class
    public static void main(String[] args) {
        Student student = new Student("Grace","Hopper",20,"CSCI","ghopper");
        System.out.println(student);
        System.out.println(student.getNameAndMajor());
        student.addGrade(new Grade("HW1", 95));
        System.out.println(student);
    }
}
