provider "azurerm" {
  features {}
  # Use the environment variables set by the azure/login action
  use_cli = false
}

terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.2.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "aida-tfstate"
    storage_account_name = "aidatfstate"
    container_name       = "tfstate"
    use_azuread_auth     = true
  }
}

locals {
  resource_prefix = "aida"
  common_tags = {
    Environment = var.environment
    Project     = "AIDA"
    ManagedBy   = "Terraform"
  }
}

resource "azurerm_resource_group" "main" {
  name     = "rg-${local.resource_prefix}-${var.environment}"
  location = "eastus"
  tags     = local.common_tags
}
