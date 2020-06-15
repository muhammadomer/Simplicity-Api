using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;

namespace  SimplicityOnlineBLL.Entities
{
	public class InvoiceItemised
	{
		public InvoiceItemised()
		{
			  InvoiceLines = new List<InvoiceItemisedItems>();
		}
		public long Sequence { get; set; }
		public string TransType { get; set; }
		public int ContactId { get; set; }
		public bool ItemisedRef { get; set; }
		public bool FlgInvoiceCreated { get; set; }
		public int SageId { get; set; }
		public string InvoiceNo { get; set; }
		public DateTime ItemisedDate { get; set; }
		public double SumAmtMain { get; set; }
		public double SumAmtLabour { get; set; }
		public double SumAmtDiscount { get; set; }
		public double SumAmtSubTotal { get; set; }
		public bool FlgIncVAT { get; set; }
		public double SumAmtVAT { get; set; }
		public bool FlgAddTax { get; set; }
		public double SumAmtTax { get; set; }
		public double SumAmtTotal { get; set; }
		public string ItemisedDetail { get; set; }
		public string SagBankCode { get; set; }
		public bool FlgChecked { get; set; }
		public DateTime? DateChecked { get; set; }
		public int CreatedBy { get; set; }
		public DateTime DateCreated { get; set; }
		public int? LastAmendedBy { get; set; }
		public DateTime? DateLastAmended { get; set; }
		public long? RossumFileSequence { get; set; }
		public string RossumPurchaseOrderoNo { get; set; }
		public string RossumDeliveryNotNo { get; set; }
		public string FileNameCabId { get; set; }
		public string SupplierName { get; set; }
		public string SupplierType { get; set; } //contains full name like 'sub-contractor'
		public List<InvoiceItemisedItems> InvoiceLines { get; set; }

	}

	public class InvoiceItemisedItems
	{
		public long Sequence { get; set; }
		public long InvoiceSequence { get; set; }
		public int ItemJoinType { get; set; }
		public DateTime ItemDate { get; set; }
		public double ItemQuantity { get; set; }
		public string ItemRef { get; set; }
		public string StockCode { get; set; }
		public string ItemDesc { get; set; }
		public string ItemUnit { get; set; }
		public int AssetSequence { get; set; }
		public double ItemAmt { get; set; }
		public double ItemAmtLabour { get; set; }
		public double ItemAmtTax { get; set; }
		public bool FlgItemDiscounted { get; set; }
		public double ItemDiscountPercent { get; set; }
		public double ItemAmtDiscount { get; set; }
		public double ItemAmtSubTotal { get; set; }
		public double ItemVATPercent { get; set; }
		public double ItemAmtVAT { get; set; }
		public double ItemAmtTotal { get; set; }
		public double ItemRetentionPercent { get; set; }
		public double ItemAmtRetention { get; set; }
		public double ItemAmtRetentionPaid { get; set; }
		public string ItemVoucherRef { get; set; }
		public string ItemRecipientName { get; set; }
		public string ItemReceiverName { get; set; }
		public string SageNominalCode { get; set; }
		public string SageTaxCode { get; set; }
		public string SageBankCode { get; set; }
		public string CostCentreId { get; set; }
		public int TelSequence { get; set; }
		public int JobSequence { get; set; }
		public bool FlgJobSeqExclude { get; set; }
		public int ItemType { get; set; }
		public int AccomSequence { get; set; }
		public bool FlgChecked { get; set; }
		public int ImportType { get; set; }
		public int ImportTypeSequence { get; set; }
		public string ImportTypeRef { get; set; }
		public string ImportTypeDescription { get; set; }
		public int CreatedBy { get; set; }
		public DateTime DateCreated { get; set; }
		public int LastAmendedBy { get; set; }
		public DateTime? DateLastAmended { get; set; }
	}
	/*
	 * Header	
			trans_type			What type of supplier it isD= SupplierC= Contractor+SubContractors
			contact_id			Entity Detail Core (edc) entityid ref column
			itemised_ref		Store default (not used usually)
			flg_val_only_inv	Store default
			Flg_inv_created		Store default (false)- true after conversion to invoice
			sum_amt_main		Sum of body table columns  Sum((item_amt - item_amt_discount) * item_quantity)
			sum_amt_labour		Sum of item_amt_labour from body table 
			sum_amt_discount	Sum of discount amount from body table  Sum(item_amt_discount)
			sum_amt_vat			Sum of item_amt_vat from body table
			sum_amt_tax			Sum of 
			sum_amt_total		Sum(item_amt_total) from body table
			sum_amt_subtotal	Sum of subtotal column from body table
			flg_inc_vat			If sum(vat) is greater > 0 then true
			itemised_detail		Invoice short notes
	
		Body table	
			item_join_type		Many other tables can have a join with this table. This column stores to which table we have to join
			item_ref			Reference column to the product table
			item_date			Invoice date
			item_unit			Ref column to table un_ref_product_units. Primary key is a character value. Match the case while storing.
			asset_segence		Store -1 (default)
			item_amt			It’s a item unit price column. Used when there a supplier or contractor. For sub contractor we'll another column in combination with this one.
			Item_amt_labour		Default 0. This column will have value if invoice is from a sub contractor.
			Other column		For all other columns store default.

	 */
}