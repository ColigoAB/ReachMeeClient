using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Coligo.ReachMee.Data.Context;
using Coligo.ReachMee.Data.Exceptions;
using Coligo.ReachMee.Data.Generics;
using Coligo.ReachMee.Data.Interfaces;
using Coligo.ReachMee.Data.Models;
using Newtonsoft.Json;

namespace Coligo.ReachMee.Data.ApiClients
{
    public class ReachMeeClientInMemory : IApiClient
    {
        #region Private Properties
        private readonly IReachMeeContext _context;
        #endregion
        #region Constructors
        public ReachMeeClientInMemory()
        {
            _context = new ReachMeeContextInMemory();
        }
        public ReachMeeClientInMemory(IReachMeeContext context)
        {
            _context = context;
        }
        #endregion
        #region Public Properties
        public Uri BaseAddress { get; set; }
        #endregion
        #region Public Methods
        public Task<HttpResponseMessage> DeleteAsync(string url)
        {
            throw new System.NotImplementedException();
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            BaseAddress = new Uri("http://localhost/" + url);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url)
            {
                RequestUri = BaseAddress,
            };

            switch (BaseAddress.Segments[4])
            {
                case "users":
                    return GetUsersAsync(httpRequestMessage);
                case "orgunits":
                    return GetOrganizationsAsync(httpRequestMessage);
                case "roles":
                    return GetRolesAsync(httpRequestMessage);
                default:
                    return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unused) { RequestMessage = httpRequestMessage });
            }
        }

        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            BaseAddress = new Uri("http://localhost/" + url);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                RequestUri = BaseAddress,
            };

            switch (BaseAddress.Segments[4])
            {
                case "users":
                    return PostUserAsync(httpRequestMessage, content);
                case "orgunits":
                    return PostOrganizationAsync(httpRequestMessage, content);
                default:
                    return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unused) { RequestMessage = httpRequestMessage });
            }

        }

        public Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            BaseAddress = new Uri("http://localhost/" + url);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, url)
            {
                RequestUri = BaseAddress,
            };

            switch (BaseAddress.Segments[4])
            {
                case "users/":
                    if (BaseAddress.Segments[6] == "roles") return PutRoleAssignmentAsync(httpRequestMessage, content);
                    return PutUserAsync(httpRequestMessage, content);
                default:
                    return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unused) { RequestMessage = httpRequestMessage });
            }
        }
        #endregion
        #region Private Methods
        private Task<HttpResponseMessage> PutUserAsync(HttpRequestMessage httpRequestMessage, HttpContent content)
        {
            throw new NotImplementedException();
        }
        private Task<HttpResponseMessage> PutRoleAssignmentAsync(HttpRequestMessage httpRequestMessage, HttpContent content)
        {
            object assignment = JsonConvert.DeserializeObject(content.ReadAsStringAsync().Result);
            try
            {
                //find user, or return 404
                var user_id = int.Parse(httpRequestMessage.RequestUri.Segments[5].Replace("/", null));

                var user = _context.GetUsers().First(x => x.User_id == user_id);
                //TODO: Fix this handling since dynamic does not work
                //_context.AddRoleAssignment(user_id, assignment assignment.external_org_unit_id);
                _context.AddRoleAssignment(user_id, "", "");
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.NoContent) { RequestMessage = httpRequestMessage });
            }
            catch (InvalidOperationException ex)
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "Candidate not found", RequestMessage = httpRequestMessage });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private Task<HttpResponseMessage> PostUserAsync(HttpRequestMessage httpRequestMessage, HttpContent content)
        {
            var user = JsonConvert.DeserializeObject<User>(content.ReadAsStringAsync().Result);
            try
            {
                _context.AddUser(user);
                HttpContent responseContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Created) { Content = responseContent, RequestMessage = httpRequestMessage });
            }
            catch (UserAlreadyExistsException ex)
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Conflict) { ReasonPhrase = ex.Message, RequestMessage = httpRequestMessage });
            }

        }
        private Task<HttpResponseMessage> GetUsersAsync(HttpRequestMessage httpRequestMessage)
        {
            try
            {
                var queryString = httpRequestMessage.RequestUri.Query;
                var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
                string user_name = queryDictionary["user_name"];
                string email = queryDictionary["email"];
                string external_org_unit_id = queryDictionary["external_org_unit_id"];
                string employee_number = queryDictionary["employee_number"];
                var pageSize = int.Parse(queryDictionary["page_size"]);
                var page = int.Parse(queryDictionary["page"]);
                var users = _context.GetUsers();
                if (!string.IsNullOrWhiteSpace(user_name)) users = users.Where(x => x.User_name == user_name).ToList();
                if (!string.IsNullOrWhiteSpace(email)) users = users.Where(x => x.Email_work == email).ToList();
                if (!string.IsNullOrWhiteSpace(external_org_unit_id)) users = users.Where(x => x.External_org_unit_id == external_org_unit_id).ToList();
                if (!string.IsNullOrWhiteSpace(employee_number)) users = users.Where(x => x.Employee_number == employee_number).ToList();
                int pages = (int)Math.Ceiling((decimal) users.Count / pageSize);
                int totaltCount = users.Count;
                int startNumber = (page - 1) * pageSize;
                bool noNextLink = false;
                if (startNumber + pageSize >= totaltCount)
                {
                    noNextLink = true;
                    pageSize = totaltCount - startNumber;
                }
                List<User> currentSet = users.GetRange(startNumber, pageSize);
                string nextLink = null;
                if (!noNextLink) nextLink = $"/public/v1/users?page={page+1}&page_size={pageSize}";
                string prevLink = null;
                if (page != 1) prevLink = $"/public/v1/users?page={page-1}&page_size={pageSize}";

                var pagedObjectResponse = new PagedResponse<User>()
                {

                    pages = pages,
                    total_count = totaltCount,
                    result = currentSet,
                    prev = prevLink,
                    next = nextLink
                };
                HttpContent content = new StringContent(JsonConvert.SerializeObject(pagedObjectResponse), Encoding.UTF8, "application/json");
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = content, RequestMessage = httpRequestMessage });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Task<HttpResponseMessage> GetRolesAsync(HttpRequestMessage httpRequestMessage)
        {
            try
            {
                var roles = _context.GetRoles();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(roles), Encoding.UTF8, "application/json");
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = content, RequestMessage = httpRequestMessage });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private Task<HttpResponseMessage> GetOrganizationsAsync(HttpRequestMessage httpRequestMessage)
        {
            try
            {
                List<Organization> organizations;
                if (httpRequestMessage.RequestUri.Query.Contains("external_id"))
                {
                    organizations = _context.GetOrganizations(httpRequestMessage.RequestUri.Query.Split(new string[] { "external_id" }, StringSplitOptions.None)[1].Split('=')[1]);
                }
                else if (httpRequestMessage.RequestUri.Query.Contains("internal_id"))
                {
                    string query = httpRequestMessage.RequestUri.Query.Split(new string[] { "internal_id" }, StringSplitOptions.None)[1].Split('=')[1];
                    int internal_id = int.Parse(query);
                    organizations = _context.GetOrganizations(query);
                }
                else
                {
                    organizations = _context.GetOrganizations();
                }

                string jsonData = JsonConvert.SerializeObject(organizations);
                HttpContent responseContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = responseContent, RequestMessage = httpRequestMessage });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Task<HttpResponseMessage> PostOrganizationAsync(HttpRequestMessage httpRequestMessage, HttpContent content)
        {
            var organization = JsonConvert.DeserializeObject<Organization>(content.ReadAsStringAsync().Result);
            try
            {
                _context.AddOrganization(organization);
                HttpContent responseContent = new StringContent(JsonConvert.SerializeObject(organization), Encoding.UTF8, "application/json");
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Created) { Content = responseContent, RequestMessage = httpRequestMessage });
            }
            catch (OrganizationAlreadyExistsException ex)
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Conflict) { ReasonPhrase = ex.Message, RequestMessage = httpRequestMessage });
            }
        }
        #endregion
    }
}
