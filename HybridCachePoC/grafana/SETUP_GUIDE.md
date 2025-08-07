# Grafana Dashboard Setup Guide

## 🎯 Overview

This guide will help you set up Grafana to monitor the HybridCache PoC performance metrics in real-time.

## 🚀 Quick Setup

### 1. Start Grafana with Docker

```bash
# Run Grafana container
docker run -d \
  --name grafana-hybridcache \
  -p 3000:3000 \
  -v $(pwd)/grafana/provisioning:/etc/grafana/provisioning \
  grafana/grafana:latest
```

### 2. Access Grafana

- **URL**: http://localhost:3000
- **Default Credentials**: admin/admin
- **Change password** when prompted

## 📊 Dashboard Setup

### Step 1: Add Data Source

1. Go to **Configuration** → **Data Sources**
2. Click **Add data source**
3. Select **SimpleJSON**
4. Configure:
   - **Name**: HybridCache PoC API
   - **URL**: http://localhost:5000
   - **Access**: Proxy
5. Click **Save & Test**

### Step 2: Import Dashboard

1. Go to **Dashboards** → **Import**
2. Copy the contents of `dashboard.json`
3. Paste into the import field
4. Click **Load**
5. Select the data source
6. Click **Import**

## 📈 Dashboard Panels

### 1. Cache Hit Rate
- **Type**: Stat
- **Data**: `/api/metrics/hit-rate`
- **Thresholds**: 
  - Red: <50%
  - Yellow: 50-80%
  - Green: >80%

### 2. Total Requests
- **Type**: Stat
- **Data**: `/api/metrics`
- **Unit**: Requests

### 3. Average Response Time
- **Type**: Stat
- **Data**: `/api/metrics`
- **Unit**: Milliseconds
- **Thresholds**:
  - Green: <50ms
  - Yellow: 50-100ms
  - Red: >100ms

### 4. Cache Size
- **Type**: Stat
- **Data**: `/api/metrics/cache-size`
- **Shows**: Memory and Redis cache sizes

### 5. Response Time by Cache Source
- **Type**: Time Series
- **Data**: `/api/metrics/response-times`
- **Shows**: Performance comparison between Memory, Redis, and Database

### 6. Cache Hit/Miss Distribution
- **Type**: Pie Chart
- **Data**: `/api/metrics/hit-rate`
- **Shows**: Visual distribution of cache hits vs misses

### 7. Request Count by Cache Source
- **Type**: Bar Chart
- **Data**: `/api/metrics/response-times`
- **Shows**: Request volume by cache source

### 8. Cache Performance Trends
- **Type**: Time Series
- **Data**: `/api/metrics` and `/api/metrics/hit-rate`
- **Shows**: Hit rate and response time trends over time

## 🔧 Custom Queries

### For SimpleJSON Data Source

#### Cache Metrics Query
```json
{
  "targets": [
    {
      "target": "cache",
      "refId": "A",
      "type": "table"
    }
  ]
}
```

#### Response Time Query
```json
{
  "targets": [
    {
      "target": "responseTimes",
      "refId": "A",
      "type": "timeseries"
    }
  ]
}
```

#### Hit Rate Query
```json
{
  "targets": [
    {
      "target": "hitRate",
      "refId": "A",
      "type": "stat"
    }
  ]
}
```

## 📊 Expected Metrics

### Performance Targets
- **Cache Hit Rate**: >80%
- **Average Response Time**: <50ms
- **Memory Cache Response**: <5ms
- **Redis Cache Response**: <50ms
- **Database Response**: <300ms

### Alert Thresholds
- **Hit Rate Alert**: <70%
- **Response Time Alert**: >100ms
- **Cache Size Alert**: >1000 items

## 🎯 Demo Scenarios

### Scenario 1: Cache Warming Impact
1. **Before Warming**: Show low hit rate and high response times
2. **After Warming**: Show improved hit rate and response times
3. **Dashboard**: Monitor the improvement in real-time

### Scenario 2: Cache Invalidation
1. **Normal Operation**: Show high hit rate
2. **Update Customer**: Trigger cache invalidation
3. **Dashboard**: Observe temporary performance dip

### Scenario 3: Load Testing
1. **Baseline**: Record normal metrics
2. **High Load**: Generate many requests
3. **Dashboard**: Monitor performance under load

## 🔍 Troubleshooting

### Common Issues

1. **No Data in Grafana**
   - Check if the API is running: `curl http://localhost:5000/api/metrics`
   - Verify data source URL is correct
   - Check network connectivity

2. **Incorrect Metrics**
   - Verify API endpoints return expected data
   - Check JSON structure matches dashboard queries
   - Review API logs for errors

3. **Dashboard Not Loading**
   - Check Grafana logs: `docker logs grafana-hybridcache`
   - Verify dashboard JSON is valid
   - Restart Grafana if needed

### Debug Commands

```bash
# Test API endpoints
curl http://localhost:5000/api/metrics
curl http://localhost:5000/api/metrics/hit-rate
curl http://localhost:5000/api/metrics/response-times

# Check Grafana logs
docker logs grafana-hybridcache

# Restart Grafana
docker restart grafana-hybridcache
```

## 🚀 Advanced Configuration

### Custom Alerts

1. **Create Alert Rules**
   - Hit Rate < 70%
   - Response Time > 100ms
   - Cache Size > 1000 items

2. **Notification Channels**
   - Email notifications
   - Slack integration
   - PagerDuty alerts

### Custom Dashboards

1. **Create New Dashboard**
   - Copy existing dashboard
   - Modify panels as needed
   - Add custom queries

2. **Export/Import**
   - Export dashboard JSON
   - Share with team
   - Version control

## 📈 Business Value

### Real-time Monitoring
- **Instant Visibility**: See cache performance in real-time
- **Proactive Alerts**: Get notified of performance issues
- **Trend Analysis**: Understand performance patterns over time

### Performance Optimization
- **Hit Rate Optimization**: Identify cache efficiency
- **Response Time Tuning**: Optimize cache configuration
- **Capacity Planning**: Plan for growth

### Cost Savings
- **Database Load Reduction**: Monitor query reduction
- **Infrastructure Optimization**: Right-size cache resources
- **Performance ROI**: Measure cache investment returns

---

**Your Grafana dashboard is now ready to impress your boss with real-time cache performance metrics! 🚀** 