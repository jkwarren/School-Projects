public class Grade {
    protected String assignmentName;
    protected int score;

    // constructor 
   public Grade (String name, int score ){

        assignmentName = name;
        this.score = score;

   }
   // get the grade from the corresponding assignment 
   public int getGrade(){
        return score;

   }

   //get the assignment from the corresponding grade
   public String getGradeName(){
    return assignmentName;
   }
   // change the grade
   public void changeGrade(int newScore){ 
        score = newScore;

   }
   public String toString() {
        String result = assignmentName + " "+ score+ " ";
        
        return result;
    //return assignmentName+ " "+ score+ " ";
}
}

