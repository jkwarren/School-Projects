// AnagramTester class for Project 3 BSTs and Anagrams
// this is the main file to run, when running you can use either text file
// small-words.txt or words.txt. Then you can input a word and get anagrams of the word
// at a max word length.

 import java.util.*;

public class AnagramTester {
    
    public static void main(String[] args) {  
        Scanner input = new Scanner(System.in);
        System.out.print("Enter name of dictionary file: ");
        String fileName = input.next();
        System.out.print("Max word length: ");
        int maxLength = input.nextInt();
        
        AnagramTree anagrams = new AnagramTree(fileName, maxLength);
        
        if (!anagrams.isEmpty()) {
            System.out.print("\nstring to search [#] to stop: ");
            String searchKey = input.next();
            while (!searchKey.equals("#")) {
                if (searchKey.length() <= maxLength) {
                    ArrayList<String> list = anagrams.findMatches(searchKey);
                    if (list != null)
                        System.out.println("  Words that match: " + list);
                    else
                        System.out.println("  NO Words match!");
                }
                else
                    System.out.println("  That word is too long; max length = " + maxLength);
                
                System.out.print("\nstring to search [#] to stop: ");
                searchKey = input.next();
             }
             input.close();

        }
    }
}