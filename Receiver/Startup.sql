if object_id('receiver.SubmittedMessage', 'U') is null
    create table receiver.SubmittedMessage (
        Id varchar(50) not null primary key,
        Value int not null
    )