﻿//< div >
//    < h4 > CategoryPost </ h4 >
//    < hr />

//    < img src = "@imageService.DecodeFile(Model.ImageData, Model.ContentType)" class= "img-fluid" />
   

//       < dl class= "row" >
    
//            < dt class= "col-sm-2" >
//                 @Html.DisplayNameFor(model => model.Title)
//             </ dt >
     
//             < dd class= "col-sm-10" >
//                  @Html.DisplayFor(model => model.Title)
//              </ dd >
      
//              < dt class= "col-sm-2" >
//                   @Html.DisplayNameFor(model => model.Abstract)
//               </ dt >
       
//               < dd class= "col-sm-10" >
//                    @Html.Raw(Model.Abstract)
//                </ dd >
        
//                < dt class= "col-sm-2" >
//                     @Html.DisplayNameFor(model => model.PostBody)
//                 </ dt >
         
//                 < dd class= "col-sm-10" >
//                      @Html.Raw(Model.PostBody)
//                  </ dd >
          
//                  < dt class= "col-sm-2" >
//                       @Html.DisplayNameFor(model => model.IsReady)
//                   </ dt >
           
//                   < dd class= "col-sm-10" >
//                        @Html.DisplayFor(model => model.IsReady)
//                    </ dd >
            
//                    < dt class= "col-sm-2" >
//                         @Html.DisplayNameFor(model => model.Created)
//                     </ dt >
             
//                     < dd class= "col-sm-10" >
//                          @Html.DisplayFor(model => model.Created)
//                      </ dd >
              
//                      < dt class= "col-sm-2" >
//                           @Html.DisplayNameFor(model => model.Updated)
//                       </ dt >
               
//                       < dd class= "col-sm-10" >
//                            @Html.DisplayFor(model => model.Updated)
//                        </ dd >
                
//                        < dt class= "col-sm-2" >
//                             @Html.DisplayNameFor(model => model.Slug)
//                         </ dt >
                 
//                         < dd class= "col-sm-10" >
//                              @Html.DisplayFor(model => model.Slug)
//                          </ dd >
                  
//                          < dt class= "col-sm-2" >
//                               @Html.DisplayNameFor(model => model.BlogCategory)
//                           </ dt >
                   
//                           < dd class= "col-sm-10" >
//                                @Html.DisplayFor(model => model.BlogCategory.Name)
//                            </ dd >
                    
//                        </ dl >
//                    </ div >
//                    < div >
                    
//                        < a asp - action = "Edit" asp - route - id = "@Model.Id" > Edit </ a > |
                         
//                             < a asp - action = "Index" > Back to List</a>
//                                </div>

//<!-- Comment section starts here-->
//<!--@if (User.Identity.IsAuthenticated)
//                                 {
//    <div class= "row" >
//        < div class= "col-8 offset-2" >
 
//             < form asp - controller = "PostComments" asp - action = "Create" >
      
//                      < div asp - validation - summary = "ModelOnly" class= "text-danger" ></ div >
             
//                             < input type = "hidden" name = "CategoryPostId" value = "@Model.Id" />
                  
//                                  < input type = "hidden" asp -for= "Slug" /> -->
                      

//                                      < !--< div class= "form-group" >
                        
//                                            < label class= "control-label" > Comment </ label >
                         
//                                             < textarea name = "CommentBody" class= "form-control" ></ textarea >
                            

//                                            </ div >
                            

//                                            < div class= "form-group" >
                             
//                                                 < input type = "submit" value = "Post" class= "btn btn-primary" />
                                  
//                                                  </ div >
                                  

//                                              </ form >
                                  

//                                          </ div >
                                  
//                                      </ div >
// }
//else
//{
//        < h2 > Login or Register to post a Comment</ h2 >
   
//           < hr />
//    }
//-->

//Where my comments will be posted

//<div class= "row" >
//    < h2 > Comment Section:</ h2 >
//   </ div >
//   @foreach(var comment in Model.PostComments.OrderByDescending(c => c.Created))
//{
//< div class= "row mt-2" >
 
//     < div class= "col" >
//          @comment.BlogUser.UserName
//      </ div >
  
//      < div class= "col align-content-start" >
//           @comment.Created.ToString("MM,dd,yyyy")

//       </ div >
   

//   </ div >
//   < div class= "row mt-2" >
    
//        < div class= "col" >
//             @comment.CommentBody
//             < hr />
     
//         </ div >
     
//         < div class= "col" >
      
//              < a asp - controller = "PostComments" asp - action = "Edit" > Edit </ a >
           
//               </ div >
           

//           </ div >

//}--> *@