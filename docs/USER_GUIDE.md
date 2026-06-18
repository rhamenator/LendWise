# LendWise User Guide

This guide is for people using LendWise to review customers, loans, follow-ups, documents, and lending work.

## What LendWise Is For

LendWise helps a lending team see:

- which customers are active
- where each loan is in the pipeline
- what follow-up work is due
- who is responsible for each task
- recent customer and loan activity

The current version uses sample data. Names, emails, phone numbers, addresses, and loan details are synthetic and are included only so the screens can be tested.

## Opening LendWise

If LendWise is already running, open a web browser and go to:

```text
http://localhost:5088
```

If the page does not open, the application may not be running yet. Ask the person managing the system to start LendWise.

## Main Navigation

The top navigation has five areas:

- **Dashboard** - summary of customers, active loans, pipeline value, overdue work, due work, and recent activity
- **Customers** - searchable list of customers and households
- **Pipeline** - loans grouped by their current stage
- **Work** - task list for follow-ups, documents, and conditions
- **Data Model** - reference view showing the major record types in the system

Most users will spend their time in Dashboard, Customers, Pipeline, and Work.

## Dashboard

Use the Dashboard as the starting point for the day.

The top numbers show:

- **Customers** - total customers in the current data set
- **Active loans** - loans currently being worked
- **Pipeline** - total value of loans in the pipeline
- **Overdue work** - tasks past their due date

The **Loan stages** section shows how many loans are in each stage and the total amount for that stage.

The **Due work** section shows urgent or upcoming tasks. Select a task row to open the related customer.

The **Recent activity** section shows the latest customer and loan events.

## Finding A Customer

Open **Customers** from the top navigation.

You can search by:

- customer or household name
- email address
- loan number

You can also filter by:

- **Heat** - how active or important the customer currently is
- **Status** - whether the customer is active, dormant, prospective, or in another status

Select **Apply** after entering search text or choosing filters.

Select a customer name to open the customer detail page.

## Customer Detail Page

The customer detail page collects the main information about one customer or household.

It shows:

- customer segment and status
- heat level
- notes
- contacts
- properties
- loans
- open work

Use this page when you need a complete view before calling a customer, reviewing a loan, or checking what work remains.

## Reviewing The Loan Pipeline

Open **Pipeline** from the top navigation.

Loans are grouped by stage. Each loan card shows:

- customer name
- loan number
- loan purpose
- loan amount
- target close date

Use the **Stage** filter to focus on one part of the pipeline, then select **Apply**.

Select a loan card to open the related customer detail page.

## Managing Work

Open **Work** from the top navigation.

The work queue shows:

- priority
- task title and description
- related customer
- due date
- assigned person
- completion status

To finish a task, select **Complete** on that row. Completed tasks are marked with a completion date.

Use **Include completed** when you want to see both open and finished tasks. Clear it when you only want the active queue.

## Understanding Priority And Heat

**Priority** belongs to a task. It tells you how urgent the work item is.

**Heat** belongs to a customer. It tells you how active, important, or time-sensitive that relationship currently is.

Use both together. A high-heat customer with high-priority work should usually be handled first.

## Data Model Page

The **Data Model** page is mainly a reference screen. It shows the major types of records LendWise is tracking, such as customers, contacts, properties, loans, work items, and activity history.

Most day-to-day users do not need this page. It is useful when discussing what information the system stores.

## Current Limitations

This version is a working product foundation, not a finished production system.

Current limitations include:

- the data is demo data only
- users cannot create new customers yet
- users cannot edit loan details yet
- completed work cannot be reopened from the screen yet
- there is no sign-in or role-based access yet

These limitations are expected at this stage.

## Good Daily Workflow

1. Open **Dashboard**.
2. Review overdue work and due work.
3. Open **Work** and complete any finished tasks.
4. Open **Pipeline** to check loans by stage.
5. Use **Customers** when you need to find a specific customer or loan number.
6. Open customer detail pages before customer calls or loan reviews.

## Getting Help

If the app does not open, a page looks wrong, or data appears missing, note:

- what page you were on
- what you searched for or clicked
- what you expected to see
- what actually happened

That information will help the support or development person reproduce the issue.
