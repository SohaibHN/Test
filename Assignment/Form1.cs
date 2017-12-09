using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LibGit2Sharp;
using System.Diagnostics;

namespace TestGit
{
    public partial class Form1 : Form
    {

        private string _repoSource;
        private UsernamePasswordCredentials _credentials;
        private DirectoryInfo _localFolder;

        public Form1()
        {
            InitializeComponent();
           String username = "sohaibhn";
            String password = "NoobRofl12";
            String gitRepoUrl = "https://github.com/SohaibHN/Test.git";
            String localFolder = "C:\\Users\\Admin\\source\\repos\\Test";

            var folder = new DirectoryInfo(localFolder);
            GitDeploy2(username, password, gitRepoUrl, localFolder);
            CommitAllChanges();
            MessageBox.Show("Commit Created");
           PushCommits("origin", "master");
            MessageBox.Show("Commit Pushed");


        }

        public void GitDeploy2(string username, string password, string gitRepoUrl, string localFolder)
        {
            var folder = new DirectoryInfo(localFolder);
            username = "ae55d53405c19be045ed5d6b242d119c465e9dcf";
            password = string.Empty;
            gitRepoUrl = "https://github.com/SohaibHN/Test.git";

            if (!folder.Exists)
            {
                throw new Exception(string.Format("Source folder '{0}' does not exist.", _localFolder));
            }

            _localFolder = folder;


            _credentials = new UsernamePasswordCredentials
            {
                Username = username,
                Password = password
            };

            _repoSource = gitRepoUrl;
        }

        /// <summary>
        /// Commits all changes.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="System.Exception"></exception>
        public void CommitAllChanges()
        {
            using (var repo = new Repository(_localFolder.FullName))
            {
                // Write content to file system
                var content = "Final TEST!";
                File.WriteAllText(Path.Combine(repo.Info.WorkingDirectory, "dhl.txt"), content);

                // Stage the file
                repo.Stage("dhl.txt");

                // Create the committer's signature and commit
                Signature author = new Signature("Sohaib", "@SohaibHN", DateTime.Now);
                Signature committer = author;

                // Commit to the repository
               Commit commit = repo.Commit("Here's a commit i made!", author, committer);

            }
        }

        /// <summary>
        /// Pushes all commits.
        /// </summary>
        /// <param name="remoteName">Name of the remote server.</param>
        /// <param name="branchName">Name of the remote branch.</param>
        /// <exception cref="System.Exception"></exception>
        public void PushCommits(string remoteName, string branchName)
        {
            using (var repo = new Repository(_localFolder.FullName))
            {
                var remote = repo.Network.Remotes.FirstOrDefault(r => r.Name == remoteName);
                if (remote == null)
                {
                    repo.Network.Remotes.Add(remoteName, _repoSource);
                    remote = repo.Network.Remotes.FirstOrDefault(r => r.Name == remoteName);
                }

                var options = new PushOptions
                {
                    CredentialsProvider = (url, usernameFromUrl, types) => _credentials
                };
                //repo.Network.Push(remote, branchName, options);
                // repo.Network.Push(repo.Branches[branchName], options);

                PushOptions po = new PushOptions();

                po.CredentialsProvider = (_url, usernameFromUrl, _cred) => _credentials;
                repo.Network.Push(remote, @"refs/heads/master", options);


            }
        }

    }
}
