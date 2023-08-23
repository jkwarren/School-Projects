// AnagramTree class for Project 3 BSTs and Anagrams
// Name: Jessica Warren

//star -> arst
// cats -> acst
//rats
// so then associated with the files...
// arst[star, rats]
// arst is connected to acst and acst[cats, cast]

//public String canonical(String s)
// //return chars id s in sorted string

import java.util.*;
import java.io.*;

public class AnagramTree {
    protected BST<String> tree;

    public AnagramTree(String filename, int maxLength) {
        // fill this in
        tree = new BST<String>();
        Scanner myFile = getFileScanner(filename); // collect from getFileScanner
        
        
        while(myFile.hasNext()){
            String originalWord = myFile.next();
            char[] sort = originalWord.toCharArray();
            Arrays.sort(sort);
            String sortedWord = Arrays.toString(sort);
            
            
            if (tree.contains(sortedWord) ){
                    
                tree.addWordToList(originalWord, sortedWord);
            }
        
            //    }
            else{
                tree.add(originalWord, sortedWord);
            }
            //}
            }
            //roster.add(s);
            
        }
        
        
        
        //tree = null; // placeholder
    

    public boolean isEmpty() {
        // fill this in
        if (tree == null){
            return true;
        }
        return false; // placeholder
    }

    public ArrayList<String> findMatches(String searchKey) {
        // fill this in
        char[] sorted = searchKey.toCharArray();
         Arrays.sort(sorted);
         String sortedWord = Arrays.toString(sorted);
        
        if (!tree.contains(sortedWord)){
            return null;
        }
        return tree.find(sortedWord); 
    }
    
    public static Scanner getFileScanner(String filename) {
        Scanner myFile;
        try { myFile = new Scanner(new FileReader(filename)); }
        catch (Exception e) {
            System.out.println("File not found: " + filename);
            return null;
        }
        return myFile;
    }
}