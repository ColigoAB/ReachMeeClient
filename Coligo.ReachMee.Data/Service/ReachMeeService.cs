using Coligo.ReachMee.Data.Exceptions;
using Coligo.ReachMee.Data.Generics;
using Coligo.ReachMee.Data.Interfaces;
using Coligo.ReachMee.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Coligo.ReachMee.Data.Service
{
    public class ReachMeeService
    {
        #region Private fields
        private readonly IApiClient _apiClient;

        private const string ApiPath = "api";
        private const string ApiPathVersion = ApiPath + "/public/v1/";
        private const string ApiPathOrganizations = ApiPathVersion + "orgunits";
        private const string ApiPathRemoveOrganization = ApiPathOrganizations + "/external_id/{0}";
        private const string ApiPathUsers = ApiPathVersion + "users";
        private const string ApiPathRoles = ApiPathVersion + "roles";
        private const string ApiPathUserRoleAssignment = ApiPathVersion + "users/{0}/roles";
        #endregion

        #region Constructors
        public ReachMeeService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        #endregion

        #region Public Methods
        #region User Management
        public User AddUser(IUser user)
        {
            var result = new HttpResponseMessage();
            try
            {
                string jsonString = JsonConvert.SerializeObject(user);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                result = _apiClient.PostAsync(ApiPathUsers, content).Result;
                if (!result.IsSuccessStatusCode)
                {
                    switch (result.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Conflict: throw new UserAlreadyExistsException(result.ReasonPhrase);
                    }
                }
                string createdContent = result.Content.ReadAsStringAsync().Result;

                User createdUser;
                try
                {
                    createdUser = JsonConvert.DeserializeObject<User>(createdContent);
                }
                catch
                {
                    var ex = new Exception($"Failed to read user ID from response: {createdContent}");
                    throw ex;
                }

                //  Return a ReachMeeUser instance with user ID populated
                return createdUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AssignRole(string role_id, int user_id, string external_org_unit_id = null)
        {
            try
            {
                var roleAssignment = new
                {
                    role_id,
                    external_org_unit_id
                };
                var jsonData = JsonConvert.SerializeObject(roleAssignment);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var result = _apiClient.PutAsync(String.Format(ApiPathUserRoleAssignment, user_id), content).Result;
                if (!result.IsSuccessStatusCode)
                {
                    switch (result.StatusCode)
                    {
                        case System.Net.HttpStatusCode.NotFound: throw new UserNotFoundException("User (or role) not found.");
                        case System.Net.HttpStatusCode.Unused: throw new NotImplementedException("Unused status code is for methods that are not implemented.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetUsers(int page_size = 50, int page = 1, string user_name = null, string email = null, string external_org_unit_id = null, string employee_number = null)
        {
            try
            {
                var users = new List<User>();
                PagedResponse<User> pagedResponse = new PagedResponse<User>();
                do
                {
                    var url = pagedResponse.next ?? ApiPathUsers + $"?page_size={page_size}&page={page}&user_name={user_name}&email={email}&external_org_unit_id={external_org_unit_id}&employee_number={employee_number}";
                    var response = _apiClient.GetAsync(url).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        switch (response.StatusCode)
                        {
                            case System.Net.HttpStatusCode.Unused: throw new NotImplementedException("Unused status code is for methods that are not implemented.");
                        }
                    }
                    string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    pagedResponse = JsonConvert.DeserializeObject<PagedResponse<User>>(content);

                    users.AddRange(pagedResponse.result);
                } while (pagedResponse.next != null);

                

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Organization Management
        public void AddOrganization(IOrganization organization)
        {
            var response = new HttpResponseMessage();
            try
            {
                string jsonString = JsonConvert.SerializeObject(organization);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                response = _apiClient.PostAsync(ApiPathOrganizations, content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Conflict: throw new OrganizationAlreadyExistsException(response.ReasonPhrase);
                        case System.Net.HttpStatusCode.InternalServerError: throw new Exception(response.ReasonPhrase);

                        case System.Net.HttpStatusCode.Unused: throw new NotImplementedException("Unused status code is for methods that are not implemented.");
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Organization> GetOrganizations()
        {
            var response = new HttpResponseMessage();
            try
            {
                response = _apiClient.GetAsync(ApiPathOrganizations).Result;
                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unused: throw new NotImplementedException("Unused status code is for methods that are not implemented.");
                    }
                }
                string jsondata = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<List<Organization>>(jsondata);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the organization, with its child organizations, with a specific external id in ReachMee 
        /// </summary>
        /// <param name="external_id">External org unit id in ReachMee</param>
        /// <returns>The corresponding organization, as a List</returns>
        public List<Organization> GetOrganization(string external_id)
        {
            var response = new HttpResponseMessage();
            try
            {
                response = _apiClient.GetAsync(ApiPathOrganizations + $"?external_id={external_id}").Result;
                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unused: throw new NotImplementedException("Unused status code is for methods that are not implemented.");
                    }
                }
                string jsondata = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<List<Organization>>(jsondata);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the organization, with its child organizations, with a specific id in ReachMee 
        /// </summary>
        /// <param name="internal_id">Org_unit_id in ReachMee</param>
        /// <returns>The corresponding organization, as a List</returns>
        public List<Organization> GetOrganization(int internal_id)
        {
            var response = new HttpResponseMessage();
            try
            {
                response = _apiClient.GetAsync(ApiPathOrganizations + $"?internal_id={internal_id}").Result;
                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unused: throw new NotImplementedException("Unused status code is for methods that are not implemented.");
                    }
                }
                string jsondata = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<List<Organization>>(jsondata);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region Role Management
        public List<Role> GetRoles()
        {
            var response = new HttpResponseMessage();
            try
            {
                response = _apiClient.GetAsync(ApiPathRoles).Result;
                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unused: throw new NotImplementedException("Unused status code is for methods that are not implemented.");
                    }
                }
                string jsonData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<List<Role>>(jsonData);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #endregion
    }
}
