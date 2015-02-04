﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace Tests
{
    public class Util
    {
        public static string TEST_REPO_DIR { get { return "test_repo"; } }
        public static string EMPTY_REPO_DIR { get { return "empty_repo"; } }

        public static void InitAll()
        {
            CleanupRepos();

            InitTestRepo();
            InitEmptyRepo();
        }

        public static Repository InitTestRepo()
        {
            if (Directory.Exists(EMPTY_REPO_DIR))
            {
                CleanupRepo(EMPTY_REPO_DIR);
            }

            Directory.CreateDirectory(TEST_REPO_DIR);
            Repository.Init(TEST_REPO_DIR);

            string readmePath = Path.Combine(TEST_REPO_DIR, "README.md");

            File.WriteAllText(readmePath, "This is a test repo");

            Repository repo = new Repository(TEST_REPO_DIR);

            repo.Index.Add("README.md");

            repo.Commit("Initial commit");

            return repo;
        }

        public static void TagTestRepo()
        {
            Repository repo = new Repository(TEST_REPO_DIR);

            repo.Tags.Add("v0.0.0", repo.Head.Tip);
        }

        public static void CleanupRepos()
        {
            (new List<string>() { TEST_REPO_DIR, EMPTY_REPO_DIR }).ForEach(x => CleanupRepo(x));
        }

        public static void CleanupRepo(string path)
        {
            if (Directory.Exists(path))
            {
                var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };

                foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
                {
                    info.Attributes = FileAttributes.Normal;
                }

                directory.Delete(true);
            }
        }

        public static Repository InitEmptyRepo()
        {
            if (Directory.Exists(EMPTY_REPO_DIR))
            {
                CleanupRepo(EMPTY_REPO_DIR);
            }

            Directory.CreateDirectory(EMPTY_REPO_DIR);
            Repository.Init(EMPTY_REPO_DIR);

            Repository repo = new Repository(EMPTY_REPO_DIR);
            return repo;
        }
    }
}
