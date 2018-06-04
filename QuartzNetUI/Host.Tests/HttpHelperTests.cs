using Host.Common;
using Host.Entity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Host.Tests
{
    public class HttpHelperTests
    {
        private string httpBase = "http://localhost:52725";

        [Fact]
        public async Task TestPostAsync()
        {
            var entity = new ScheduleEntity();
            entity.TriggerType = TriggerTypeEnum.Simple;
            entity.JobName = "JobNameBenny";
            entity.JobGroup = "JobGroupBenny";
            entity.IntervalSecond = 12;
            var addUrl = httpBase + "/api/Job/AddJob";
            //��Ӳ�������
            var resultStr = await HttpHelper.Instance.PostAsync(addUrl, JsonConvert.SerializeObject(entity));
            var addResult = JsonConvert.DeserializeObject<BaseResult>(resultStr);
            
            //��֤
            Assert.True(addResult.Code == 200);

            //ɾ����������
            var key = new JobKey(entity.JobName, entity.JobGroup);
            var delUrl = httpBase + "/api/Job/RemoveJob";
            var delResultStr = await HttpHelper.Instance.PostAsync(delUrl, JsonConvert.SerializeObject(key));
            var delResult = JsonConvert.DeserializeObject<BaseResult>(delResultStr);
            Assert.True(delResult.Code == 200);
        }

        [Fact]
        public async Task TestGetAsync()
        {
            var url = httpBase + "/api/Job/GetAllJob";
            var obj = await HttpHelper.Instance.GetAsync(url);
            var result = JsonConvert.DeserializeObject<List<JobBriefInfoEntity>>(obj);
            Assert.True(result != null);
        }

        [Fact]
        public async Task TestPutAsync()
        {
            var url = "http://localhost:50090/api/Values/123";
            var obj = await HttpHelper.Instance.PutAsync(url, "{Version:123}");
        }

        [Fact]
        public async Task TestDeleteAsync()
        {
            var url = "http://localhost:50090/api/Values/123";
            var obj = await HttpHelper.Instance.DeleteAsync(url);
        }
    }
}
