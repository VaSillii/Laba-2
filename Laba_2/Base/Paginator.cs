using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Laba_2.Base
{
    public class Paginator
    {
        private int itemsPerPage;
        private int currentPage;
        private List<Risk> risksItems = new List<Risk>();

        public Paginator(List<Risk> risksItems)
        {
            this.risksItems = risksItems;
            this.itemsPerPage = 15;
        }

        public int CurrentPage {
            get
            {
               return currentPage;
            } 
            set 
            {
                if (value < 0)
                {
                    this.currentPage = 0;
                } else if (value > PageCount)
                {
                    this.currentPage = PageCount;
                }
                else
                {
                    this.currentPage = value; 
                } 
            }
        }

        public int Count
        {
            get
            {
                if (this.risksItems.Count == 0) return 0;
                if (this.CurrentPage < this.PageCount && this.risksItems.Count >= this.itemsPerPage)
                {
                    return this.itemsPerPage;
                }
                else
                {
                    var itemsLeft = this.risksItems.Count % this.itemsPerPage;
                    if (itemsLeft > 0 || itemsLeft < this.itemsPerPage)
                    {
                        return itemsLeft;
                    } else if (itemsLeft == 0)
                    {
                        return this.itemsPerPage;
                    }
                    else
                    {
                        return itemsLeft;
                    }
                }
            }
        }

        public int PageCount
        {
            get
            {
                return (this.risksItems.Count + this.itemsPerPage - 1) / this.itemsPerPage;
            }
        }

        public int ItemPerPage {
            get
            {
                if (this.risksItems.Count == 0) return 0;
                if (this.CurrentPage < this.PageCount)
                {
                    return this.itemsPerPage;
                }
                else
                {
                    var itemsLeft = this.risksItems.Count % this.itemsPerPage;
                    if (itemsLeft == 0)
                    {
                        return this.itemsPerPage;
                    }
                    else
                    {
                        return itemsLeft;
                    }
                }
            }
            set
            {
                this.itemsPerPage = value;
            }
        } 

        public List<Risk> MoveToNextPage()
        {
            if (this.CurrentPage < this.PageCount)
            {
                this.CurrentPage += 1;
            }
            return GeneratePage(CurrentPage);
        }

        public List<Risk> MoveToPreviousPage()
        {
            if (this.CurrentPage > 1)
            {
                this.CurrentPage -= 1;
            }
            return GeneratePage(CurrentPage);
        }

        public List<Risk> GeneratePage(int pageNumber)
        {
            this.CurrentPage = pageNumber;
            List<Risk> threadItemsPage = new List<Risk>();

            int indexStart = 0;
            if (this.CurrentPage == this.PageCount)
            {
                indexStart = (this.PageCount - 1) * this.itemsPerPage;
            }
            else if (this.CurrentPage == 0)
            {
                indexStart *= this.itemsPerPage;
            }
            else
            {
               indexStart = (this.CurrentPage - 1) * this.itemsPerPage;
            }

            for (int i = indexStart; i < indexStart + this.Count; i++)
            {
                threadItemsPage.Add(this.risksItems[i]);
            }
            return threadItemsPage;
        }

        public void UpdateCollectionRisks(List<Risk> risks)
        {
            this.risksItems = risks;
        }
    }
}
