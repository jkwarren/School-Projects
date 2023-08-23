// GradeBook class for P1-GradeBook
// Name: Jessica Warren
// Date: November 2022
// Section (A or B):  A

//This is an example gradebook, it takes a roster of students with their graduation
// year, username, major, and grades

// Things that can be printed: print grades by ID, print by major, change homework scores,
//     and removing people from the roster

import java.util.*;
import java.io.*;

public class GradeBook {

    private ArrayList<Student> roster;
    
    
    // Default constructor
    public GradeBook() {
        roster = new ArrayList<Student>();
    }


    // Constructor fills roster with data from file
    public GradeBook(String fileName) {
        // your code goes here
        roster = new ArrayList<Student>();
        Scanner myFile = getFileScanner(fileName); // collect from getFileScanner
        
        while(myFile.hasNextLine()){
            
            String line = myFile.nextLine();
            String[] data = line.split(" ");
            Student s = new Student(data[0], data[1], Integer.parseInt(data[2]), data[3], data[4]);
            for (int x = 5; x < data.length; x = x+ 2){
                s.addGrade(new Grade (data[x], Integer.parseInt( data[x + 1]))); // adds the grades to gradebook

            }
            roster.add(s);
            
        }
        myFile.close();
        
       
    }
    
    // String representation of GradeBook
    public String toString() {
        String result = "";
        for (Student x: roster){
            result += x.toString();
        }
        return result;
        //return roster.toString();  // a way to get started
    }
    
    
    public void printIndividualGrades(String id){
        boolean personFound = false;
        for (int p = 0 ;  p < roster.size() ; p ++ ){
            Student x = roster.get(p);   //collecting the student to look at
            if (x.getUsername().equals(id) ){    // if the IDs match we print
                personFound = true;
                System.out.println(x.getGradesList().toString());
            }
            
        }
            if (personFound == false){ // if the person was not found we print out nothing
                
                    System.out.print("");
            }
        
        
    }

    //Prints all the grades, nicely formatted, that are associated with each student that is
    // in major major. Prints nothing if no entries match the specified major.
    public void printGradesByMajor(String major){
        boolean peopleFound = false;
        for (int p = 0 ;  p < roster.size() ; p ++ ){
            Student x = roster.get(p);   //collecting the student to look at
            if (x.getMajor().equals(major) ){    // if the IDs match we print
                peopleFound = true;
                System.out.println(x.getGradesList().toString());
            }
            
        }
        if (peopleFound == false){ // if the people were not found we print out nothing
                
            System.out.print("");
    }
    }
    // Removes the entry for the student with username, id, from the roster, 
    //using the appropriate ArrayList method. If there is no such student, 
    //prints an appropriate error message.
    public void removeStudent(String id){
        boolean personFound = false;
        for (int p = 0 ;  p < roster.size() ; p ++ ){
            Student x = roster.get(p);   //collecting the student to look at
            if (x.getID().equals(id) ){    // if the IDs match we print
                personFound = true;
                roster.remove(p);
                
            }
        }
        if (personFound == false){ // if the people were not found we print out nothing
                
            System.out.print("Username not found, please enter valid ID");
        }
    }

    //If a student with username id exists, changes the grade associated with assignment to newScore.
    // Prints an appropriate error message if either the id or assignment cannot be found.
    public void changeGrade(String id, String assignment, int newScore ){
        boolean idFound = false;
        boolean assignmentFound = false;
        for (int p = 0 ;  p < roster.size() ; p ++ ){
            Student x = roster.get(p);   //collecting the student to look at

            if (x.getID().equals(id) ){    // if the IDs match we print
                idFound = true;
                 ArrayList<Grade>xsGrades = x.getGrades();

                for (int k = 0; k < xsGrades.size(); k ++){
                    
                    Grade grade = xsGrades.get(k);

                    if (grade.getGradeName().equals(assignment)){
                        assignmentFound = true;
                        grade.changeGrade(newScore);
                    }
                        
                }
                
            }
        }
        if (idFound == false ){ // if the people were not found we print out nothing
                
            System.out.print("Username not found, please enter valid ID");
        }
        if (assignmentFound == false){
            System.out.print("Assignment not found, please enter valid Assignment");
        }
    }


    // Helper methods to open files for reading/writing
    public static Scanner getFileScanner(String filename) {
        Scanner myFile;
        try { myFile = new Scanner(new FileReader(filename)); }
        catch (Exception e) {
            System.out.println("File not found: " + filename);
            return null;
        }
        return myFile;
    }
    
    public static PrintWriter getFileWriter(String filename) {
        PrintWriter outFile;
        try { outFile = new PrintWriter(filename); }
        catch (Exception e) {
            System.out.println("Error opening file: " + filename);
            return null;
        }
        return outFile;
    }
    
    // Testing the GradeBook class
    public static void main(String[] args) {
        System.out.println("printing all from roster");
        GradeBook grades = new GradeBook("data/F22grades.txt");
        System.out.println(grades);
        // add method calls here to test your code
        System.out.println("Here we print ID: pahmad grades");
        grades.printIndividualGrades("pahmad");

        System.out.println("Here we print the CSCI majors Grades");
        grades.printGradesByMajor("CSCI");

        System.out.println("Changing pahmad assignment HW1 to score: 100");
        grades.changeGrade("pahmad", "HW1", 100 );
        grades.printIndividualGrades("pahmad");

        System.out.println("Removing pahmad from the roster");
        grades.removeStudent("pahmad");
        System.out.println(grades);
        
    }
    
}
