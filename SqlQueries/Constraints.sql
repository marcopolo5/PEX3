ALTER TABLE [dbo].[Blocked_Users]  WITH CHECK ADD  CONSTRAINT [FK_Block_List_Users] FOREIGN KEY([userid])
REFERENCES [dbo].[Users] ([id])

ALTER TABLE [dbo].[Blocked_Users] CHECK CONSTRAINT [FK_Block_List_Users]



ALTER TABLE [dbo].[Blocked_Users]  WITH CHECK ADD  CONSTRAINT [FK_Block_List_Users1] FOREIGN KEY([blockeduserid])
REFERENCES [dbo].[Users] ([id])

ALTER TABLE [dbo].[Blocked_Users] CHECK CONSTRAINT [FK_Block_List_Users1]



ALTER TABLE [dbo].[Friend_Requests]  WITH CHECK ADD  CONSTRAINT [FK_Friend_Requests_Receiver] FOREIGN KEY([receiverid])
REFERENCES [dbo].[Users] ([id])

ALTER TABLE [dbo].[Friend_Requests] CHECK CONSTRAINT [FK_Friend_Requests_Receiver]



ALTER TABLE [dbo].[Friend_Requests]  WITH CHECK ADD  CONSTRAINT [FK_Friend_Requests_Sender] FOREIGN KEY([senderid])
REFERENCES [dbo].[Users] ([id])

ALTER TABLE [dbo].[Friend_Requests] CHECK CONSTRAINT [FK_Friend_Requests_Sender]



ALTER TABLE [dbo].[Friends]  WITH CHECK ADD  CONSTRAINT [FK_Friends_Users] FOREIGN KEY([userid])
REFERENCES [dbo].[Users] ([id])

ALTER TABLE [dbo].[Friends] CHECK CONSTRAINT [FK_Friends_Users]



ALTER TABLE [dbo].[Friends]  WITH CHECK ADD  CONSTRAINT [FK_Friends_Users1] FOREIGN KEY([friendid])
REFERENCES [dbo].[Users] ([id])

ALTER TABLE [dbo].[Friends] CHECK CONSTRAINT [FK_Friends_Users1]



ALTER TABLE [dbo].[Group_Members]  WITH CHECK ADD  CONSTRAINT [FK_Group_Members_Conversations] FOREIGN KEY([conversationid])
REFERENCES [dbo].[Conversations] ([id])

ALTER TABLE [dbo].[Group_Members] CHECK CONSTRAINT [FK_Group_Members_Conversations]



ALTER TABLE [dbo].[Group_Members]  WITH CHECK ADD  CONSTRAINT [FK_Group_Members_Users] FOREIGN KEY([userid])
REFERENCES [dbo].[Users] ([id])

ALTER TABLE [dbo].[Group_Members] CHECK CONSTRAINT [FK_Group_Members_Users]



ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Messages] FOREIGN KEY([conversationid])
REFERENCES [dbo].[Conversations] ([id])

ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Messages]



ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Users] FOREIGN KEY([senderid])
REFERENCES [dbo].[Users] ([id])

ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Users]



ALTER TABLE [dbo].[Profiles]  WITH CHECK ADD  CONSTRAINT [FK_Profiles_Users] FOREIGN KEY([userid])
REFERENCES [dbo].[Users] ([id])

ALTER TABLE [dbo].[Profiles] CHECK CONSTRAINT [FK_Profiles_Users]



ALTER TABLE [dbo].[Settings]  WITH CHECK ADD  CONSTRAINT [FK_Settings_Users] FOREIGN KEY([userid])
REFERENCES [dbo].[Users] ([id])

ALTER TABLE [dbo].[Settings] CHECK CONSTRAINT [FK_Settings_Users]



ALTER TABLE [dbo].[Strikes] WITH CHECK ADD CONSTRAINT [FK_Strikes_Users] FOREIGN KEY([userid])
REFERENCES [dbo].[Users] ([id])

ALTER TABLE [dbo].[Strikes] CHECK CONSTRAINT [FK_Strikes_Users]



ALTER TABLE [dbo].[UserReports]  WITH CHECK ADD  CONSTRAINT [FK_UserReports_Reporter] FOREIGN KEY([userid])
REFERENCES [dbo].[Users] ([id])

ALTER TABLE [dbo].[UserReports] CHECK CONSTRAINT [FK_UserReports_Reporter]



ALTER TABLE [dbo].[UserReports]  WITH CHECK ADD  CONSTRAINT [FK_UserReports_Reportee] FOREIGN KEY([reporteduserid])
REFERENCES [dbo].[Users] ([id])

ALTER TABLE [dbo].[UserReports] CHECK CONSTRAINT [FK_UserReports_Reportee]