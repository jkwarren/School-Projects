import java.util.ArrayList;

// BST class for Project 3 BSTs and Anagrams
// Name: Jessica Warren

public class BST<E extends Comparable<E>> {
    private TreeNode root;
    
    public BST()     {
        root = null;
    }
    
    public boolean isEmpty()     {
        return root == null;
    }
    
    public void add(E value) {
        root = addHelper(root, value, null);
    }
    public void add(E originalValue, E sortedValue){
        root = addHelper(root, sortedValue, originalValue);
    }
    private TreeNode addHelper(TreeNode rt, E value, E originalValue) {
        if (rt == null){
            // this is to add values to a node's list if it is that kind of tree
            TreeNode n = new TreeNode(value, null, null);
            if (originalValue != null){
                n.words.add(originalValue);
                //System.out.print(n.words);
            }
            return n;

        }
        if (value.compareTo(rt.data) < 0)
            rt.left = addHelper(rt.left, value, originalValue);
        else if (value.compareTo(rt.data) > 0)
            rt.right = addHelper(rt.right, value, originalValue);
        else
            throw new IllegalStateException("Duplicate value in tree " + value);
        
        return rt;  
    }
    
            
    public void inOrder() {
        inOrder(root);
    }
    
    private void inOrder(TreeNode rt) {
        if (rt != null) {
            inOrder(rt.left);
            System.out.print(rt.data + " ");
            inOrder(rt.right);
        }
    }
    
    public void preOrder() {
        preOrder(root);
    }
    
    private void preOrder(TreeNode rt) {
        if (rt != null) {
            System.out.print(rt.data + " ");
            preOrder(rt.left);
            preOrder(rt.right);
        }
    }
    
    public int size() {
        return size(root);
    }
    
    private int size(TreeNode rt) {
        if (rt == null)
            return 0;
        return 1 + size(rt.left) + size(rt.right);
    }
    
    public boolean contains(E value) {
        TreeNode rt = root;
        while (rt != null) {
            if (value.compareTo(rt.data) == 0)
                return true;
            else if (value.compareTo(rt.data) < 0)
                rt = rt.left;
            else
                rt = rt.right;
        }
        return false;
    }

    // added methods for the assignment 
    public int height(){
        return height(root);
    }

    private int height(TreeNode rt){

        if (rt == null ){
            return -1;
        }
        
        return 1 + Math.max(height(rt.left), height(rt.right));
    }


    public boolean isBalanced(){
        return isBalanced(root);
    }

    private boolean isBalanced(TreeNode rt){
        
        //if the root is null, then we return true
        if (rt == null){
            return true;
        }
        // if the height of the left sub tree minus the height 
        //of the right sub tree is between -2 and 2,
        //then check right sub balanced and left sub balanced
        int heightLeft = height(rt.left);
        int heightRight = height(rt.right);
        int diffInHeight = heightLeft - heightRight;

        if(diffInHeight < 2 && diffInHeight > -2){
            return isBalanced(rt.left) && isBalanced(rt.right);
        }
        // if any part is not balanced, return false
        return false;
        
    }

    @Override
    public boolean equals(Object tree) {
        if (this == tree) return true;
        if (tree == null || !(tree instanceof BST<?>)) return false;
        BST<?> otherTree = BST.class.cast(tree);
        return equals(this.root, otherTree.root);
    }

    private boolean equals(TreeNode root1, BST<?>.TreeNode root2) {
        // fill this in to test if root1 equals root2
        // if both are empty, return true
        if (root1 == null && root2 == null){
            return true;
        }
        else if (root1 ==null || root2 == null){
            return false;
        }

        // if root1 value and root2 values are different, false otherwise continue
        if (root1.data == root2.data){
            return equals(root1.left, root2.left) && equals(root1.right, root2.right);
        }

        return false; 
    }
    
    // takes a sorted value and determines if it is in the tree as a key
    // and returns the key's list of original values
    public ArrayList<E> find(E sortedValue){
        return find(root, sortedValue);
    }
    private ArrayList<E> find( TreeNode rt,E sortedValue){
        if (!contains(sortedValue)){
            ArrayList<E> n = new ArrayList<E>();
            n.add(rt.data);
            return n;
        }
        // transverse the tree till equals is true then take that
        //node and return its  words
        if (rt.data.equals(sortedValue)){
            return rt.words;
        }
        else if (sortedValue.compareTo(rt.data) < 0){
                rt = rt.left;
        }
        else{
            rt = rt.right;
        }
        return find(rt, sortedValue);

    }
    public void addWordToList(E originalValue, E sortedValue){
        find(sortedValue).add(originalValue);
    }

    // returns a String that prints tree top to bottom, right to left in a 90-degree rotated level view
    public String toString() {
        StringBuilder result =  new StringBuilder();
        return toString(result, -1, root).toString();
    }
    
    public StringBuilder toString(StringBuilder res, int height, TreeNode rt) {
        if (rt != null) {
            height++;
            res = toString(res, height, rt.right);
            for (int i = 0; i < height; i++)
                res.append("\t");
            res.append(rt.data + "\n");
            res = toString(res, height, rt.left);
        }
        return res;
    }
    
    // The TreeNode class is a private inner class used (only) by the BST class
    private class TreeNode {
        private E data;
        // add new variable BST<E> tree;
        private TreeNode left, right;
        private ArrayList <E> words; //ArrayList<E> words
        
        private TreeNode(E data, TreeNode left, TreeNode right) {
            this.data = data;
            this.left = left;
            this.right = right;
            words = new ArrayList<E>();
        }
    }
    
    public static void main(String[] args) {
        BST<Integer> treeTest = new BST<Integer>();
        treeTest.add(7);
        treeTest.add(5);
        treeTest.add(4);
        treeTest.add(10);
        treeTest.add(6);
        treeTest.add(8);

        treeTest.add(3);
        //treeTest.add(2);

        treeTest.inOrder();
        System.out.println();
        treeTest.preOrder();
        System.out.println(treeTest.size());
        System.out.println();
        System.out.println(treeTest.contains(6));
        System.out.println(treeTest.contains(112));
        System.out.println(treeTest.contains(7));
        System.out.println(treeTest.contains(10));
        System.out.println();
        System.out.println(treeTest);

        // testing height
        System.out.println("Testing height of tree");
        System.out.print("Tree height: ");
        System.out.println(treeTest.height());

        // testing is balanced
        System.out.println();
        System.out.println("Testing is tree is balanced");
        System.out.println(treeTest.isBalanced());

        // testing equals
        BST<Integer> treeTest2 = new BST<Integer>();
        treeTest2.add(7);
        treeTest2.add(5);
        treeTest2.add(4);
        treeTest2.add(10);
        treeTest2.add(6);
        treeTest2.add(8);


        System.out.println();
        System.out.println("Testing equals");
        System.out.println(treeTest.equals(treeTest2));

        // testing new add to see if it can handle 
        //just a value then a sorted and unsorted
        BST<String> treeTest3 = new BST<String>();
        treeTest3.add("snot", "nost");
        treeTest3.add("cat");
        treeTest3.add("dog", "dgo");
        
        System.out.println();
        System.out.println("Testing new add method");
        System.out.println(treeTest3);

        // test if find works
        System.out.println("Testing new find method for nost");
        System.out.println(treeTest3.find("nost"));
        


    }
    
}
