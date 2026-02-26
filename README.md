private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
{
    MessageBox.Show($"Text={e.Node.Text}\nTag={(e.Node.Tag==null ? "null" : e.Node.Tag.GetType().Name)}");
}
